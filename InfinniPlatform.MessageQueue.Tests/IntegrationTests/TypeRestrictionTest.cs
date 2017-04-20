using System;

using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [Category(TestCategories.UnitTest)]
    public class TypeRestrictionTest : RabbitMqTestBase
    {
        [Test]
        public void BroadcastProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var broadcastProducer = new RabbitMqBroadcastProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => broadcastProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => broadcastProducer.PublishAsync(new DynamicWrapper()));
        }

        [Test]
        public void TaskProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var taskProducer = new RabbitMqTaskProducer(RabbitMqManager, RabbitMqMessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => taskProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => taskProducer.PublishAsync(new DynamicWrapper()));
        }
    }
}