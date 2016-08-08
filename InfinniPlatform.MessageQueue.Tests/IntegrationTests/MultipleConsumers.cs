using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Consumers;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MultipleConsumers : RabbitMqTestBase
    {
        [Test]
        public void MessageAreDividedWithinConsumers()
        {
            var messageSerializer = new MessageSerializer();

            var actualMessagesLists = new List<List<DynamicWrapper>>
                                      {
                                          new List<DynamicWrapper>(),
                                          new List<DynamicWrapper>(),
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

            var completeEvent = new CountdownEvent(assertMessages.Length);

            ITaskConsumer[] taskConsumers =
            {
                new DynamicWrapperTaskConsumer(actualMessagesLists[0], completeEvent),
                new DynamicWrapperTaskConsumer(actualMessagesLists[1], completeEvent),
                new DynamicWrapperTaskConsumer(actualMessagesLists[2], completeEvent),
                new DynamicWrapperTaskConsumer(actualMessagesLists[3], completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer, new Mock<ILog>().Object);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicWrapper).FullName);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");

            var actualMessages = new List<DynamicWrapper>();
            foreach (var list in actualMessagesLists)
            {
                CollectionAssert.IsNotEmpty(list);
                actualMessages.AddRange(list);
            }

            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}