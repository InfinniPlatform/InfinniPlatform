using System.Threading.Tasks;

using InfinniPlatform.Core;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class OnDemandConsumerTest : RabbitMqTestBase
    {
        [Test]
        public async Task OnDemandConsumerReturnsMessageOrNull()
        {
            var messageSerializer = new MessageSerializer();
            var onDemandConsumer = new OnDemandConsumer(RabbitMqManager, messageSerializer);

            DynamicWrapper[] assertMessages =
            {
                new DynamicWrapper { { "SomeField", "Message1" } },
                new DynamicWrapper { { "SomeField", "Message2" } },
                new DynamicWrapper { { "SomeField", "Message3" } }
            };

            var producerBase = new TaskProducer(RabbitMqManager, messageSerializer, new AppIdentity());
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicWrapper).FullName);
            }

            foreach (var message in assertMessages)
            {
                var actualMessage = await onDemandConsumer.Consume<DynamicWrapper>();
                var body = actualMessage.GetBody();
                Assert.AreEqual(message, body);
            }

            var emptyMessage = await onDemandConsumer.Consume<DynamicWrapper>();
            Assert.IsNull(emptyMessage);
        }
    }
}