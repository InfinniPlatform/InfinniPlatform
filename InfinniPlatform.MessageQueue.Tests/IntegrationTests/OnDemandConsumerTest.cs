﻿using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class OnDemandConsumerTest : RabbitMqTestBase
    {
        [Test]
        public void OnDemandConsumerReturnsMessageOrNull()
        {
            var messageSerializer = new MessageSerializer();
            var onDemandConsumer = new OnDemandConsumer(RabbitMqManager, messageSerializer);

            DynamicWrapper[] assertMessages =
            {
                new DynamicWrapper { { "SomeField", "Message1" } },
                new DynamicWrapper { { "SomeField", "Message2" } },
                new DynamicWrapper { { "SomeField", "Message3" } }
            };

            var producerBase = new ProducerBase(RabbitMqManager, messageSerializer);
            foreach (var message in assertMessages)
            {
                producerBase.Produce(new Message<DynamicWrapper>(message));
            }

            foreach (var message in assertMessages)
            {
                var actualMessage = onDemandConsumer.Consume<DynamicWrapper>();
                var body = actualMessage.GetBody();
                Assert.AreEqual(message, body);
            }

            var emptyMessage = onDemandConsumer.Consume<DynamicWrapper>();
            Assert.IsNull(emptyMessage);
        }
    }
}