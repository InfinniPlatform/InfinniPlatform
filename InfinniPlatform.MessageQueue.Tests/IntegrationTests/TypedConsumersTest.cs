using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.TestConsumers;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class TypedConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void AllDynamicDocumentMessagesDelivered()
        {
            var actualMessages = new List<DynamicDocument>();
            DynamicDocument[] assertMessages =
            {
                new DynamicDocument { { "Field1", "string" } },
                new DynamicDocument { { "Field2", 1 } },
                new DynamicDocument { { "Field3", new DateTime(1, 1, 1) } },
                new DynamicDocument { { "Field4", 1.2 } },
                new DynamicDocument { { "Field5", false } }
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);
            ITaskConsumer[] taskConsumers =
            {
                new DynamicDocumentTaskConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicDocument).FullName);
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

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
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

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
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