using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Hosting;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;

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
            var messageSerializer = new MessageSerializer();

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

            IConsumer[] listOfConsumers =
            {
                new DynamicWrapperTaskConsumer(actualMessagesLists[0], completeEvent1, 1000),
                new DynamicWrapperTaskConsumer(actualMessagesLists[1], completeEvent2)
            };

            var messageConsumersManager = new MessageConsumersManager(listOfConsumers, RabbitMqManager, messageSerializer, new Mock<ILog>().Object);
            messageConsumersManager.OnAfterStart();

            var producerBase = new TaskProducerBase(RabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Publish(message);
            }

            const int timeout = 5000;
            Assert.IsTrue(completeEvent1.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");
            Assert.IsTrue(completeEvent2.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");

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