using System;
using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Consumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class RoutingTest : RabbitMqTestBase
    {
        [Test]
        public void AllMessagesRoutedToCorrespondedNamedQueues()
        {
            var messageSerializer = new MessageSerializer();

            var queue1Messages = new List<DynamicWrapper>();
            var queue1AssertMessages = new List<DynamicWrapper>
                                       {
                                           new DynamicWrapper { { "SomeField", "Message1" } },
                                           new DynamicWrapper { { "SomeField", "Message2" } },
                                           new DynamicWrapper { { "SomeField", "Message3" } }
                                       };
            var queue1CountdownEvent = new CountdownEvent(3);
            var queue1TaskConsumer = new Queue1DynamicWrapperTaskConsumer(queue1Messages, queue1CountdownEvent);

            var queue2Messages = new List<DynamicWrapper>();
            var queue2AssertMessages = new List<DynamicWrapper>
                                       {
                                           new DynamicWrapper { { "SomeField", "Message4" } },
                                           new DynamicWrapper { { "SomeField", "Message5" } }
                                       };
            var queue2CountdownEvent = new CountdownEvent(2);
            var queue2TaskConsumer = new Queue2DynamicWrapperTaskConsumer(queue2Messages, queue2CountdownEvent);

            var queue3Messages = new List<DynamicWrapper>();
            var queue3AssertMessages = new List<DynamicWrapper> { new DynamicWrapper { { "SomeField", "Message6" } } };
            var queue3CountdownEvent = new CountdownEvent(1);
            var queue3TaskConsumer = new Queue3DynamicWrapperTaskConsumer(queue3Messages, queue3CountdownEvent);

            ITaskConsumer[] taskConsumers =
            {
                queue1TaskConsumer,
                queue2TaskConsumer,
                queue3TaskConsumer
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer);
            foreach (var message in queue1AssertMessages)
            {
                producerBase.Publish(message, "Queue1");
            }
            foreach (var message in queue2AssertMessages)
            {
                producerBase.Publish(message, "Queue2");
            }
            foreach (var message in queue3AssertMessages)
            {
                producerBase.Publish(message, "Queue3");
            }

            const int timeout = 500;
            Assert.IsTrue(queue1CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            Assert.IsTrue(queue2CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            Assert.IsTrue(queue3CountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");

            CollectionAssert.AreEquivalent(queue1AssertMessages, queue1Messages);
            CollectionAssert.AreEquivalent(queue2AssertMessages, queue2Messages);
            CollectionAssert.AreEquivalent(queue3AssertMessages, queue3Messages);
        }

        [Test]
        public void AllTypedMessagesRoutedToCorrespondedConsumers()
        {
            var messageSerializer = new MessageSerializer();

            var dynamicWrapperMessages = new List<DynamicWrapper>();
            var dynamicWrapperCountdownEvent = new CountdownEvent(3);
            var dynamicWrapperConsumer = new DynamicWrapperTaskConsumer(dynamicWrapperMessages, dynamicWrapperCountdownEvent);

            var testMessages = new List<TestMessage>();
            var testMessageCountdownEvent = new CountdownEvent(2);
            var testMessageConsumer = new TestMessageTaskConsumer(testMessages, testMessageCountdownEvent);

            var stringMessages = new List<string>();
            var stringCountdownEvent = new CountdownEvent(1);
            var stringConsumer = new StringTaskConsumer(stringMessages, stringCountdownEvent);

            object[] assertMessages =
            {
                new DynamicWrapper { { "SomeField", "Message1" } },
                new DynamicWrapper { { "SomeField", "Message2" } },
                new DynamicWrapper { { "SomeField", "Message3" } },
                new TestMessage("1", 1, new DateTime(1, 1, 1)),
                new TestMessage("2", 2, new DateTime(2, 2, 2)),
                "message1"
            };

            ITaskConsumer[] taskConsumers =
            {
                dynamicWrapperConsumer,
                stringConsumer,
                testMessageConsumer
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 500;
            Assert.IsTrue(dynamicWrapperCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            Assert.IsTrue(stringCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            Assert.IsTrue(testMessageCountdownEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");

            var actualMessages = new List<object>();
            actualMessages.AddRange(dynamicWrapperMessages);
            actualMessages.AddRange(stringMessages);
            actualMessages.AddRange(testMessages);

            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}