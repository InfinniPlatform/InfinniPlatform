using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.TestConsumers;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class RoutingTest : RabbitMqTestBase
    {
        [Test]
        public void AllMessagesRoutedToCorrespondedNamedQueues()
        {
            var queue1Messages = new List<DynamicDocument>();
            var queue1AssertMessages = new List<DynamicDocument>
                                       {
                                           new DynamicDocument { { "SomeField", "Message1" } },
                                           new DynamicDocument { { "SomeField", "Message2" } },
                                           new DynamicDocument { { "SomeField", "Message3" } }
                                       };
            var queue1CountdownEvent = new CountdownEvent(3);
            var queue1TaskConsumer = new Queue1DynamicDocumentTaskConsumer(queue1Messages, queue1CountdownEvent);

            var queue2Messages = new List<DynamicDocument>();
            var queue2AssertMessages = new List<DynamicDocument>
                                       {
                                           new DynamicDocument { { "SomeField", "Message4" } },
                                           new DynamicDocument { { "SomeField", "Message5" } }
                                       };
            var queue2CountdownEvent = new CountdownEvent(2);
            var queue2TaskConsumer = new Queue2DynamicDocumentTaskConsumer(queue2Messages, queue2CountdownEvent);

            var queue3Messages = new List<DynamicDocument>();
            var queue3AssertMessages = new List<DynamicDocument>
                                       {
                                           new DynamicDocument { { "SomeField", "Message6" } }
                                       };
            var queue3CountdownEvent = new CountdownEvent(1);
            var queue3TaskConsumer = new Queue3DynamicDocumentTaskConsumer(queue3Messages, queue3CountdownEvent);

            ITaskConsumer[] taskConsumers =
            {
                queue1TaskConsumer,
                queue2TaskConsumer,
                queue3TaskConsumer
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in queue1AssertMessages)
            {
                producerBase.PublishDynamic(message, "Queue1");
            }
            foreach (var message in queue2AssertMessages)
            {
                producerBase.PublishDynamic(message, "Queue2");
            }
            foreach (var message in queue3AssertMessages)
            {
                producerBase.PublishDynamic(message, "Queue3");
            }

            const int timeout = 500;
            Assert.IsTrue(queue1CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(queue1TaskConsumer)}.");
            Assert.IsTrue(queue2CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(queue2TaskConsumer)}.");
            Assert.IsTrue(queue3CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(queue3TaskConsumer)}.");

            CollectionAssert.AreEquivalent(queue1AssertMessages, queue1Messages);
            CollectionAssert.AreEquivalent(queue2AssertMessages, queue2Messages);
            CollectionAssert.AreEquivalent(queue3AssertMessages, queue3Messages);
        }

        [Test]
        public void AllTypedMessagesRoutedToCorrespondedBroadcastConsumers()
        {
            var namedQueueMessages = new List<TestMessage>();
            var namedQueueCountdownEvent = new CountdownEvent(0);
            var namedQueueConsumer = new NamedQueueTestMessageBroadcastConsumer(namedQueueMessages, namedQueueCountdownEvent);

            var testMessages = new List<TestMessage>();
            var testMessageCountdownEvent = new CountdownEvent(6);
            var testMessageConsumer = new TestMessageBroadcastConsumer(testMessages, testMessageCountdownEvent);

            TestMessage[] assertMessages =
            {
                new TestMessage("1", 1, new DateTime(1, 1, 1)),
                new TestMessage("2", 2, new DateTime(2, 2, 2)),
                new TestMessage("3", 3, new DateTime(3, 3, 3)),
                new TestMessage("4", 4, new DateTime(4, 4, 4)),
                new TestMessage("5", 5, new DateTime(5, 5, 5)),
                new TestMessage("6", 6, new DateTime(6, 6, 6))
            };

            IBroadcastConsumer[] broadcastConsumers =
            {
                namedQueueConsumer,
                testMessageConsumer
            };

            RegisterConsumers(null, broadcastConsumers);

            var producerBase = new RabbitMqBroadcastProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;

            Assert.IsTrue(namedQueueCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(namedQueueConsumer)}.");
            Assert.IsTrue(testMessageCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(testMessageConsumer)}.");

            CollectionAssert.AreEquivalent(Enumerable.Empty<TestMessage>(), namedQueueMessages);
            CollectionAssert.AreEquivalent(assertMessages, testMessages);
        }

        [Test]
        public void AllTypedMessagesRoutedToCorrespondedTaskConsumers()
        {
            var dynamicDocumentMessages = new List<DynamicDocument>();
            var dynamicDocumentCountdownEvent = new CountdownEvent(3);
            var dynamicDocumentConsumer = new DynamicDocumentTaskConsumer(dynamicDocumentMessages, dynamicDocumentCountdownEvent);

            var testMessages = new List<TestMessage>();
            var testMessageCountdownEvent = new CountdownEvent(2);
            var testMessageConsumer = new TestMessageTaskConsumer(testMessages, testMessageCountdownEvent);

            var stringMessages = new List<string>();
            var stringCountdownEvent = new CountdownEvent(1);
            var stringConsumer = new StringTaskConsumer(stringMessages, stringCountdownEvent);

            object[] assertMessages =
            {
                new DynamicDocument { { "SomeField", "Message1" } },
                new DynamicDocument { { "SomeField", "Message2" } },
                new DynamicDocument { { "SomeField", "Message3" } },
                new TestMessage("1", 1, new DateTime(1, 1, 1)),
                new TestMessage("2", 2, new DateTime(2, 2, 2)),
                "message1"
            };

            ITaskConsumer[] taskConsumers =
            {
                dynamicDocumentConsumer,
                stringConsumer,
                testMessageConsumer
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;

            Assert.IsTrue(dynamicDocumentCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(dynamicDocumentConsumer)}.");
            Assert.IsTrue(stringCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(stringConsumer)}.");
            Assert.IsTrue(testMessageCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms by {nameof(testMessageConsumer)}.");

            var actualMessages = new List<object>();
            actualMessages.AddRange(dynamicDocumentMessages);
            actualMessages.AddRange(stringMessages);
            actualMessages.AddRange(testMessages);

            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}