using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Dynamic;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class TypedConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void AllDynamicWrapperMessagesDelivered()
        {
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
            ITaskConsumer[] taskConsumers =
            {
                new DynamicWrapperTaskConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicWrapper).FullName);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }

        [Test]
        public void AllStringMessagesDelivered()
        {
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
                new StringTaskConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }

        [Test]
        public void AllTestMessagesDelivered()
        {
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
            ITaskConsumer[] taskConsumers =
            {
                new TestMessageTaskConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            CollectionAssert.AreEquivalent(actualMessages, actualMessages);
        }
    }
}