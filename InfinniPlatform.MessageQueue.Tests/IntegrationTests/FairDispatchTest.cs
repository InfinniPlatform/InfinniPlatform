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
    public class FairDispatchTest : RabbitMqTestBase
    {
        [Test]
        public void MessagesDispatchedInFairManner()
        {
            var actualMessagesLists = new List<List<DynamicDocument>>
                                      {
                                          new List<DynamicDocument>(),
                                          new List<DynamicDocument>()
                                      };

            DynamicDocument[] assertMessages =
            {
                new DynamicDocument { { "SomeField", "Message1" } },
                new DynamicDocument { { "SomeField", "Message2" } },
                new DynamicDocument { { "SomeField", "Message3" } },
                new DynamicDocument { { "SomeField", "Message4" } },
                new DynamicDocument { { "SomeField", "Message5" } },
                new DynamicDocument { { "SomeField", "Message6" } },
                new DynamicDocument { { "SomeField", "Message7" } },
                new DynamicDocument { { "SomeField", "Message8" } }
            };

            var consumer1MessageCount = 1;
            var consumer2MessageCount = assertMessages.Length - 1;
            var completeEvent1 = new CountdownEvent(consumer1MessageCount);
            var completeEvent2 = new CountdownEvent(consumer2MessageCount);

            ITaskConsumer[] taskConsumers =
            {
                new DynamicDocumentTaskConsumer(actualMessagesLists[0], completeEvent1, 4000),
                new DynamicDocumentTaskConsumer(actualMessagesLists[1], completeEvent2)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicDocument).FullName);
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent1.Wait(timeout), $"Failed finish {consumer1MessageCount} message consuming by slow consumer in {timeout} ms.");
            Assert.IsTrue(completeEvent2.Wait(timeout), $"Failed finish {consumer2MessageCount} message consuming by fast consumer in {timeout} ms.");

            var actualMessages = new List<DynamicDocument>();
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