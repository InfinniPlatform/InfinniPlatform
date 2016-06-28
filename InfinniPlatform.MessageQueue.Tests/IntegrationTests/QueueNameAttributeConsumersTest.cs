﻿using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Queues.Consumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class QueueNameAttributeConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void AllStringMessagesDelivered()
        {
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
            ITaskConsumer[] taskConsumers =
            {
                new StringTaskConsumerWithAttribute(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message, "StringConsumerWithAttributeTest");
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }

        [Test]
        public void AllTestMessagesDelivered()
        {
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
            ITaskConsumer[] taskConsumers =
            {
                new TestMessageWithAttributeTaskConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}