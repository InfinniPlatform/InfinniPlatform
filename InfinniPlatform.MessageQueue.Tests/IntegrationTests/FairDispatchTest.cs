using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Core.Dynamic;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.MessageQueue.RabbitMQ;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class FairDispatchTest : RabbitMqTestBase
    {
        [Test]
        public void MessagesDispatchedInFairManner()
        {
            var actualMessagesLists = new List<List<DynamicWrapper>>
                                      {
                                          new List<DynamicWrapper>(),
                                          new List<DynamicWrapper>()
                                      };

            DynamicWrapper[] assertMessages =
            {
                new DynamicWrapper { { "SomeField", "Message1" } },
                new DynamicWrapper { { "SomeField", "Message2" } },
                new DynamicWrapper { { "SomeField", "Message3" } },
                new DynamicWrapper { { "SomeField", "Message4" } },
                new DynamicWrapper { { "SomeField", "Message5" } },
                new DynamicWrapper { { "SomeField", "Message6" } },
                new DynamicWrapper { { "SomeField", "Message7" } },
                new DynamicWrapper { { "SomeField", "Message8" } }
            };

            var consumer1MessageCount = 1;
            var consumer2MessageCount = assertMessages.Length - 1;
            var completeEvent1 = new CountdownEvent(consumer1MessageCount);
            var completeEvent2 = new CountdownEvent(consumer2MessageCount);

            ITaskConsumer[] taskConsumers =
            {
                new DynamicWrapperTaskConsumer(actualMessagesLists[0], completeEvent1, 4000),
                new DynamicWrapperTaskConsumer(actualMessagesLists[1], completeEvent2)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicWrapper).FullName);
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent1.Wait(timeout), $"Failed finish {consumer1MessageCount} message consuming by slow consumer in {timeout} ms.");
            Assert.IsTrue(completeEvent2.Wait(timeout), $"Failed finish {consumer2MessageCount} message consuming by fast consumer in {timeout} ms.");

            var actualMessages = new List<DynamicWrapper>();
            foreach (var list in actualMessagesLists)
            {
                actualMessages.AddRange(list);
            }

            Assert.AreEqual(actualMessagesLists[0].Count, consumer1MessageCount);
            Assert.AreEqual(actualMessagesLists[1].Count, consumer2MessageCount);

            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}