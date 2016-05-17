using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Queues;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class QueueNameAttributeConsumersTest
    {
        [Test]
        public void AllStringMessagesDelivered()
        {
            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);
            var messageSerializer = new MessageSerializer();

            var actualMessages = new List<string>();
            string[] assertMessages =
            {
                "message1",
                "message2",
                "message3",
                "message4",
                "message5"
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);
            IConsumer[] listOfConsumers =
            {
                new StringConsumerWithAttribute(actualMessages, completeEvent)
            };

            var messageConsumersManager = new MessageConsumersManager(rabbitMqManager, listOfConsumers, messageSerializer);
            messageConsumersManager.OnAfterStart();

            var producerBase = new ProducerBase(rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<string>(message), "StringConsumerWithAttributeTest");
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }

        [Test]
        public void AllTestMessagesDelivered()
        {
            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);
            var messageSerializer = new MessageSerializer();

            var actualMessages = new List<TestMessageWithAttribute>();
            TestMessageWithAttribute[] assertMessages =
            {
                new TestMessageWithAttribute("1"),
                new TestMessageWithAttribute("2"),
                new TestMessageWithAttribute("3"),
                new TestMessageWithAttribute("4"),
                new TestMessageWithAttribute("5")
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);
            IConsumer[] listOfConsumers =
            {
                new TestMessageWithAttributeConsumer(actualMessages, completeEvent), 
            };

            var messageConsumersManager = new MessageConsumersManager(rabbitMqManager, listOfConsumers, messageSerializer);
            messageConsumersManager.OnAfterStart();

            var producerBase = new ProducerBase(rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<TestMessageWithAttribute>(message));
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}