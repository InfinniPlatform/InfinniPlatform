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
    public class MultipleConsumersTest : RabbitMqTestBase
    {
        [Test]
        public void MessageAreDividedWithinConsumers()
        {
            var actualMessagesLists = new List<List<DynamicDocument>>
            {
                new List<DynamicDocument>(),
                new List<DynamicDocument>(),
                new List<DynamicDocument>(),
                new List<DynamicDocument>()
            };

            DynamicDocument[] assertMessages =
            {
                new DynamicDocument {{"SomeField", "Message1"}},
                new DynamicDocument {{"SomeField", "Message2"}},
                new DynamicDocument {{"SomeField", "Message3"}},
                new DynamicDocument {{"SomeField", "Message4"}},
                new DynamicDocument {{"SomeField", "Message5"}},
                new DynamicDocument {{"SomeField", "Message6"}},
                new DynamicDocument {{"SomeField", "Message7"}},
                new DynamicDocument {{"SomeField", "Message8"}}
            };

            var completeEvent = new CountdownEvent(assertMessages.Length);

            ITaskConsumer[] taskConsumers =
            {
                new DynamicDocumentTaskConsumer(actualMessagesLists[0], completeEvent),
                new DynamicDocumentTaskConsumer(actualMessagesLists[1], completeEvent),
                new DynamicDocumentTaskConsumer(actualMessagesLists[2], completeEvent),
                new DynamicDocumentTaskConsumer(actualMessagesLists[3], completeEvent)
            };

            RegisterConsumers(taskConsumers, null);

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicDocument).FullName);
            }

            const int timeout = 500;
            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms.");

            var actualMessages = new List<DynamicDocument>();
            foreach (var list in actualMessagesLists)
            {
                CollectionAssert.IsNotEmpty(list);
                actualMessages.AddRange(list);
            }

            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}