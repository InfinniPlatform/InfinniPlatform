using System;
using System.Text;
using System.Threading;

using InfinniPlatform.Helpers;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;

using NUnit.Framework;

using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class ConnectionTest
    {
        private const string ApplicationName = "RabbitTest";
        private const int WaitTimeout = 30000;
        private const string ServiceName = "RabbitMQ";

        [OneTimeSetUp]
        public void SetUp()
        {
            WindowsServices.StartService(ServiceName, WaitTimeout);
        }

        [Test]
        public void AutoRecconectAfterServerRestart()
        {
            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, ApplicationName);
            var connection = rabbitMqManager.GetConnection();

            Assert.DoesNotThrow(() => connection.CreateModel());

            WindowsServices.StopService(ServiceName, WaitTimeout);

            Assert.Throws<AlreadyClosedException>(() => connection.CreateModel());

            WindowsServices.StartService(ServiceName, WaitTimeout);
            Thread.Sleep(5000);

            Assert.DoesNotThrow(() => connection.CreateModel());
        }

        [Test]
        public void ResumePublishingProcessAfterServerRestart()
        {
            var message = Encoding.UTF8.GetBytes(DateTime.Now.ToString("s"));

            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, ApplicationName);
            var connection = rabbitMqManager.GetConnection();

            var model = connection.CreateModel();
            model.QueueDeclare("auto_reconnect_test", false, false, false, null);

            var eventingBasicConsumer = new EventingBasicConsumer(model);
            eventingBasicConsumer.Received += (sender, args) => Assert.AreEqual(message, Encoding.UTF8.GetString(args.Body));

            model.BasicPublish("", "auto_reconnect_test", null, message);
            model.BasicConsume("auto_reconnect_test", true, eventingBasicConsumer);

            WindowsServices.StopService(ServiceName, WaitTimeout);

            Assert.Throws<AlreadyClosedException>(() => model.BasicPublish("", "auto_reconnect_test", null, message));

            WindowsServices.StartService(ServiceName, WaitTimeout);
            Thread.Sleep(5000);

            Assert.DoesNotThrow(() => model.BasicPublish("", "auto_reconnect_test", null, message));
        }
    }
}