using System;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    public class TypeRestrictionTest : RabbitMqTestBase
    {
        [Test]
        public void BroadcastProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var broadcastProducer = new BroadcastProducer(RabbitMqManager, new MessageSerializer());

            Assert.Throws<ArgumentException>(() => broadcastProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => broadcastProducer.PublishAsync(new DynamicWrapper()));
        }

        [Test]
        public void TaskProducerThrowsExceptionIfDynamicWrapperSendViaPublishMethod()
        {
            var taskProducer = new TaskProducer(RabbitMqManager, new MessageSerializer());

            Assert.Throws<ArgumentException>(() => taskProducer.Publish(new DynamicWrapper()));
            Assert.ThrowsAsync<ArgumentException>(() => taskProducer.PublishAsync(new DynamicWrapper()));
        }
    }
}