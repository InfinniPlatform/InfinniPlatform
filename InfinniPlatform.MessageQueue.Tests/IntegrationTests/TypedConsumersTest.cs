using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class TypedConsumersTest
    {
        private static RabbitMqManager _rabbitMqManager;

        [OneTimeSetUp]
        public void Init()
        {
            _rabbitMqManager = new RabbitMqManager(RabbitMqConnectionSettings.Default, TestConstants.ApplicationName);
            var enumerable = _rabbitMqManager.GetQueues();
            _rabbitMqManager.DeleteQueues(enumerable);
        }

        [Test]
        public void AllDynamicWrapperMessagesDelivered()
        {
            var messageSerializer = new MessageSerializer();

            var actualMessages = new List<DynamicWrapper>();
            DynamicWrapper[] assertMessages =
            {
                new DynamicWrapper { { "Field1", "string" } },
                new DynamicWrapper { { "Field2", 1 } },
                new DynamicWrapper { { "Field3", new DateTime(1, 1, 1) } },
                new DynamicWrapper { { "Field4", 1.2 } },
                new DynamicWrapper { { "Field5", false } }
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);
            IConsumer[] listOfConsumers =
            {
                new DynamicWrapperConsumer(actualMessages, completeEvent)
            };

            var messageConsumersManager = new MessageConsumersManager(_rabbitMqManager, listOfConsumers, messageSerializer);
            messageConsumersManager.OnAfterStart();

            var producerBase = new ProducerBase(_rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<DynamicWrapper>(message));
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }

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
            IConsumer[] listOfConsumers =
            {
                new StringConsumer(actualMessages, completeEvent)
            };

            var messageConsumersManager = new MessageConsumersManager(_rabbitMqManager, listOfConsumers, messageSerializer);
            messageConsumersManager.OnAfterStart();

            var producerBase = new ProducerBase(_rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<string>(message));
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

            var actualMessages = new List<TestMessage>();
            TestMessage[] assertMessages =
            {
                new TestMessage("1", 1, new DateTime(1, 1, 1)),
                new TestMessage("2", 2, new DateTime(2, 2, 2)),
                new TestMessage("3", 3, new DateTime(3, 3, 3)),
                new TestMessage("4", 4, new DateTime(4, 4, 4)),
                new TestMessage("5", 5, new DateTime(5, 5, 5))
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);
            IConsumer[] listOfConsumers =
            {
                new TestMessageConsumer(actualMessages, completeEvent)
            };

            var messageConsumersManager = new MessageConsumersManager(rabbitMqManager, listOfConsumers, messageSerializer);
            messageConsumersManager.OnAfterStart();

            var producerBase = new ProducerBase(rabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<TestMessage>(message));
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(actualMessages, actualMessages);
        }
    }
}