using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.TestConsumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class MultipleConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void MessageAreDividedWithinConsumers()
        {
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

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
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