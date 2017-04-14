using System;
using InfinniPlatform.MessageQueue.RabbitMQ;
using InfinniPlatform.Sdk.Dynamic;

using Moq;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [Category(TestCategories.UnitTest)]
    public class TypeRestrictionTest : RabbitMqTestBase
    {
        [Test]
        public void BroadcastProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var broadcastProducer = new BroadcastProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => broadcastProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => broadcastProducer.PublishAsync(new DynamicWrapper()));
        }

        [Test]
        public void TaskProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var taskProducer = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => taskProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => taskProducer.PublishAsync(new DynamicWrapper()));
        }
    }
}