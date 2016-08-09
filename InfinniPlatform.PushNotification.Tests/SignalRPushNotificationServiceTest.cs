using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.NodeServiceHost;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.PushNotification;

using Microsoft.AspNet.SignalR.Client;

using NUnit.Framework;

namespace InfinniPlatform.PushNotification.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Manual because heavy")]
    public sealed class SignalRPushNotificationServiceTest
    {
        public SignalRPushNotificationServiceTest()
        {
            _currentDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? ".";
        }


        private readonly string _currentDirectory;
        private ServiceHost _serviceHost;


        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            Directory.SetCurrentDirectory(_currentDirectory);

            AppDomain.CurrentDomain.AssemblyResolve += OnAssemblyResolve;

            _serviceHost = new ServiceHost();
            _serviceHost.Start(Timeout.InfiniteTimeSpan);
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            _serviceHost.Stop(Timeout.InfiniteTimeSpan);
            _serviceHost = null;

            AppDomain.CurrentDomain.AssemblyResolve -= OnAssemblyResolve;

            Directory.SetCurrentDirectory(".");
        }


        private static Assembly OnAssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assemblyFileName = new AssemblyName(args.Name).Name + ".dll";
            var currentDirectory = Path.GetDirectoryName(new Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath) ?? ".";
            var assemblyFilePath = Directory.EnumerateFiles(currentDirectory, assemblyFileName, SearchOption.AllDirectories).FirstOrDefault();
            return !string.IsNullOrEmpty(assemblyFilePath) ? Assembly.LoadFile(assemblyFilePath) : null;
        }


        [Test]
        public async Task ShouldNotifyAll()
        {
            // Given

            var pushNotificationService = _serviceHost.HostingContext.ContainerResolver.Resolve<IPushNotificationService>();

            var receiveEvent = new CountdownEvent(3 + 2 + 1);

            var client1 = new SignalRClient(_serviceHost.HostingContext.Configuration, receiveEvent)
                .Subscribe("key1");

            var client2 = new SignalRClient(_serviceHost.HostingContext.Configuration, receiveEvent)
                .Subscribe("key1")
                .Subscribe("key2");

            var client3 = new SignalRClient(_serviceHost.HostingContext.Configuration, receiveEvent)
                .Subscribe("key1")
                .Subscribe("key2")
                .Subscribe("key3");

            // When

            using (await client1.Listen())
            using (await client2.Listen())
            using (await client3.Listen())
            {
                await pushNotificationService.NotifyAll("key1", "message1");
                await pushNotificationService.NotifyAll("key2", "message2");
                await pushNotificationService.NotifyAll("key3", "message3");

                Assert.IsTrue(receiveEvent.Wait(5000));
            }

            // Then

            CollectionAssert.AreEquivalent(new[] { "message1" }, client1.Messages);
            CollectionAssert.AreEquivalent(new[] { "message1", "message2" }, client2.Messages);
            CollectionAssert.AreEquivalent(new[] { "message1", "message2", "message3" }, client3.Messages);
        }


        private class SignalRClient
        {
            public SignalRClient(HostingConfig hostingConfig, CountdownEvent receiveEvent)
            {
                _hostingConfig = hostingConfig;
                _receiveEvent = receiveEvent;
                _subscriptions = new List<string>();
                _messages = new List<object>();
            }


            private readonly HostingConfig _hostingConfig;
            private readonly CountdownEvent _receiveEvent;
            private readonly List<string> _subscriptions;
            private readonly List<object> _messages;


            public IEnumerable<object> Messages => _messages;


            public SignalRClient Subscribe(string messageType)
            {
                _subscriptions.Add(messageType);

                return this;
            }


            public async Task<IDisposable> Listen()
            {
                var hubConnection = new HubConnection($"{_hostingConfig.Scheme}://{_hostingConfig.Name}:{_hostingConfig.Port}/");
                var hubProxy = hubConnection.CreateHubProxy("SignalRPushNotificationServiceHub");

                foreach (var messageType in _subscriptions)
                {
                    hubProxy.On<object>(messageType, OnReceive);
                }

                await hubConnection.Start();

                return hubConnection;
            }


            private void OnReceive(object message)
            {
                _messages.Add(message);
                _receiveEvent.Signal();
            }
        }
    }
}