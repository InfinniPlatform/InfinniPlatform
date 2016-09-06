using System;
using System.Text;
using System.Threading;

using InfinniPlatform.Helpers;

using NUnit.Framework;

using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    [Ignore("Due to results inconsistency.")]
    public class ConnectionTest : RabbitMqTestBase
    {
        [Test]
        public void AutoRecconectAfterServerRestart()
        {
            Assert.DoesNotThrow(() => RabbitMqManager.GetChannel());

            WindowsServices.StopService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.Throws<AggregateException>(() => RabbitMqManager.GetChannel());

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.DoesNotThrow(() => RabbitMqManager.GetChannel());
        }

        [Test]
        public void ResumePublishingProcessAfterServerRestart()
        {
            const string routingKey = "auto_reconnect_test";

            var message = Encoding.UTF8.GetBytes(DateTime.Now.ToString("s"));

            var channel = RabbitMqManager.GetChannel();

            channel.QueueDeclare(routingKey, false, false, false, null);

            var eventingBasicConsumer = new EventingBasicConsumer(channel);
            eventingBasicConsumer.Received += (sender, args) => Assert.AreEqual(message, Encoding.UTF8.GetString(args.Body));

            channel.BasicPublish(string.Empty, routingKey, true, null, message);
            channel.BasicConsume(routingKey, true, "", false, false, null, eventingBasicConsumer);

            WindowsServices.StopService(TestConstants.ServiceName, TestConstants.WaitTimeout);

            Assert.Throws<AlreadyClosedException>(() => channel.BasicPublish(string.Empty, routingKey, true, null, message));

            WindowsServices.StartService(TestConstants.ServiceName, TestConstants.WaitTimeout);
            Thread.Sleep(10000);
            Assert.DoesNotThrow(() => channel.BasicPublish(string.Empty, routingKey, true, null, message));
        }
    }
}