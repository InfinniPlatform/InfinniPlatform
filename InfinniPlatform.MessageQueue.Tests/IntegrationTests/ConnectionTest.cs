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
    public class ConnectionTest : RabbitMqTestBase
    {
        [Test]
        public void AutoRecconectAfterServerRestart()
        {
            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);

            Assert.DoesNotThrow(() => rabbitMqManager.GetChannel());

            WindowsServices.StopService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.Throws<AlreadyClosedException>(() => rabbitMqManager.GetChannel());

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.DoesNotThrow(() => rabbitMqManager.GetChannel());
        }

        [Test]
        public void ResumePublishingProcessAfterServerRestart()
        {
            var message = Encoding.UTF8.GetBytes(DateTime.Now.ToString("s"));

            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);

            var channel = rabbitMqManager.GetChannel();
            channel.QueueDeclare("auto_reconnect_test", false, false, false, null);

            var eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += (sender, args) => Assert.AreEqual(message, Encoding.UTF8.GetString(args.Body));

            channel.BasicPublish("", "auto_reconnect_test", null, message);
            channel.BasicConsume("auto_reconnect_test", true, eventingBasicConsumer);

            WindowsServices.StopService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.Throws<AlreadyClosedException>(() => channel.BasicPublish("", "auto_reconnect_test", null, message));

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);
            Thread.Sleep(5000);
            Assert.DoesNotThrow(() => channel.BasicPublish("", "auto_reconnect_test", null, message));
        }
    }
}