﻿using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.UnitTest)]
    public class FanoutConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void EachFanoutConsumerRecieveAllMessages()
        {
            var rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);
            var messageSerializer = new MessageSerializer();

            TestMessage[] assertMessages =
            {
                new TestMessage("1", 1, new DateTime(1, 1, 1)),
                new TestMessage("2", 2, new DateTime(2, 2, 2)),
                new TestMessage("3", 3, new DateTime(3, 3, 3)),
                new TestMessage("4", 4, new DateTime(4, 4, 4)),
                new TestMessage("5", 5, new DateTime(5, 5, 5))
            };

            var actualMessages1 = new List<TestMessage>();
            var completeEvent1 = new CountdownEvent(assertMessages.Length);
            var fanoutConsumer1 = new TestMessageBroadcastConsumer(actualMessages1, completeEvent1);

            var actualMessages2 = new List<TestMessage>();
            var completeEvent2 = new CountdownEvent(assertMessages.Length);
            var fanoutConsumer2 = new TestMessageBroadcastConsumer(actualMessages2, completeEvent2);

            IConsumer[] listOfConsumers =
            {
                fanoutConsumer1,
                fanoutConsumer2
            };

            var messageConsumersManager = new MessageConsumersManager(listOfConsumers, rabbitMqManager, messageSerializer, new Mock<ILog>().Object);
            messageConsumersManager.OnAfterStart();

            var producerBase = new BroadcastProducerBase(rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent1.Wait(timeout), $"Failed finish message consuming in {timeout} ms for {nameof(fanoutConsumer1)}.");
            Assert.IsTrue(completeEvent2.Wait(timeout), $"Failed finish message consuming in {timeout} ms for {nameof(fanoutConsumer2)}.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages1);
            CollectionAssert.AreEquivalent(assertMessages, actualMessages2);
        }
    }
}