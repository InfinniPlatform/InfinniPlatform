using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Dynamic;

using Moq;

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
            var onDemandConsumer = new OnDemandConsumer(RabbitMqManager, MessageSerializer);

            DynamicWrapper[] assertMessages =
            {
                new DynamicWrapper { { "SomeField", "Message1" } },
                new DynamicWrapper { { "SomeField", "Message2" } },
                new DynamicWrapper { { "SomeField", "Message3" } }
            };

            var producerBase = new TaskProducer(RabbitMqManager, MessageSerializer, new Mock<IBasicPropertiesProvider>().Object);
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