using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Api;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SignalR.Modules;

using Microsoft.AspNet.SignalR.Client;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.SignalR.Tests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Sometimes SignalR stops asynchronously")]
    public sealed class SignalROwinHostingModuleIntegrationTest
    {
        private static IOwinHostingContext CreateTestOwinHostingContext(params IOwinHostingModule[] owinHostingModules)
        {
            var containerResolverMoq = new Mock<IContainerResolver>();
            containerResolverMoq.Setup(i => i.Resolve<IEnumerable<IOwinHostingModule>>()).Returns(owinHostingModules);

            var hostingContextMoq = new Mock<IOwinHostingContext>();
            hostingContextMoq.SetupGet(i => i.Configuration).Returns(HostingConfig.Default);
            hostingContextMoq.SetupGet(i => i.ContainerResolver).Returns(containerResolverMoq.Object);

            return hostingContextMoq.Object;
        }


        [Test]
        public void CheckCommunications()
        {
            var owinHostingContext = CreateTestOwinHostingContext(new SignalROwinHostingModule());
            var owinHostingService = new OwinHostingService(owinHostingContext);

            owinHostingService.Start();

            try
            {
                // Направленное оповещение от клиента клиенту

                {
                    // Given

                    var receiveEvent1 = new CountdownEvent(1);
                    var receiveEvent2 = new CountdownEvent(1);

                    var client1 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent1, "someEvent1");
                    var client2 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent2, "someEvent2");

                    // When

                    client1.Notify("someEvent2", "1to2");
                    client2.Notify("someEvent1", "2to1");

                    receiveEvent1.Wait(5000);
                    receiveEvent2.Wait(5000);

                    // Then

                    CollectionAssert.AreEquivalent(new[] { "1to2" }, client2.ReceiveMessages);
                    CollectionAssert.AreEquivalent(new[] { "2to1" }, client1.ReceiveMessages);
                }

                // Оповещение от сервера всем клиентам

                {
                    // Given

                    var receiveEvent = new CountdownEvent(3);

                    var server = new WebClientNotificationProxy();

                    var client1 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent, "someEvent1");
                    var client2 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent, "someEvent1");
                    var client3 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent, "someEvent1");

                    // When

                    server.Notify("someEvent1", "eventBody1");

                    receiveEvent.Wait(5000);

                    // Then

                    CollectionAssert.AreEquivalent(new[] { "eventBody1" }, client1.ReceiveMessages);
                    CollectionAssert.AreEquivalent(new[] { "eventBody1" }, client2.ReceiveMessages);
                    CollectionAssert.AreEquivalent(new[] { "eventBody1" }, client3.ReceiveMessages);
                }

                // Оповещения от сервера определенным клиентам

                {
                    // Given

                    var receiveEvent1 = new CountdownEvent(1);
                    var receiveEvent2 = new CountdownEvent(1);
                    var receiveEvent3 = new CountdownEvent(1);

                    var server = new WebClientNotificationProxy();

                    var client1 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent1, "someEvent1");
                    var client2 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent2, "someEvent2");
                    var client3 = new WebClientNotification(owinHostingContext.Configuration, receiveEvent3, "someEvent3");

                    // When

                    server.Notify("someEvent1", "eventBody1");
                    server.Notify("someEvent2", "eventBody2");
                    server.Notify("someEvent3", "eventBody3");

                    receiveEvent1.Wait(5000);
                    receiveEvent2.Wait(5000);
                    receiveEvent3.Wait(5000);

                    // Then

                    CollectionAssert.AreEquivalent(new[] { "eventBody1" }, client1.ReceiveMessages);
                    CollectionAssert.AreEquivalent(new[] { "eventBody2" }, client2.ReceiveMessages);
                    CollectionAssert.AreEquivalent(new[] { "eventBody3" }, client3.ReceiveMessages);
                }
            }
            finally
            {
                owinHostingService.Stop();
            }
        }


        private class WebClientNotification
        {
            public WebClientNotification(HostingConfig hostingConfig, CountdownEvent receiveEvent, string routingKey)
            {
                var hubConnection = new HubConnection($"{hostingConfig.Scheme}://{hostingConfig.Name}:{hostingConfig.Port}/");
                var hubProxy = hubConnection.CreateHubProxy("WebClientNotificationHub");
                hubProxy.On<object>(routingKey, OnReceive);
                hubConnection.Start().Wait();

                _receiveEvent = receiveEvent;
                _hubProxy = hubProxy;
            }


            private readonly IHubProxy _hubProxy;
            private readonly CountdownEvent _receiveEvent;
            public readonly List<object> ReceiveMessages = new List<object>();


            public void Notify(string routingKey, object message)
            {
                _hubProxy.Invoke("Notify", routingKey, message);
            }


            private void OnReceive(object message)
            {
                ReceiveMessages.Add(message);

                _receiveEvent.Signal();
            }
        }
    }
}