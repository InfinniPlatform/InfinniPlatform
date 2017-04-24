using System.Threading.Tasks;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class OnDemandConsumerTest : RabbitMqTestBase
    {
        [Test]
        public async Task OnDemandConsumerReturnsMessageOrNull()
        {
            var onDemandConsumer = new RabbitMqOnDemandConsumer(RabbitMqManager, RabbitMqMessageSerializer);

            DynamicDocument[] assertMessages =
            {
                new DynamicDocument { { "SomeField", "Message1" } },
                new DynamicDocument { { "SomeField", "Message2" } },
                new DynamicDocument { { "SomeField", "Message3" } }
            };

            var producerBase = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                producerBase.PublishDynamic(message, typeof(DynamicDocument).FullName);
            }

            foreach (var message in assertMessages)
            {
                var actualMessage = await onDemandConsumer.Consume<DynamicDocument>();
                var body = actualMessage.GetBody();
                Assert.AreEqual(message, body);
            }

            var emptyMessage = await onDemandConsumer.Consume<DynamicDocument>();
            Assert.IsNull(emptyMessage);
        }
    }
}