﻿using System;
using InfinniPlatform.Dynamic;
using InfinniPlatform.Tests;
using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.IntegrationTests
{
    [Category(TestCategories.UnitTest)]
    public class TypeRestrictionTest : RabbitMqTestBase
    {
        [Test]
        public void BroadcastProducerThrowsExceptionIfDynamicDocumentSendViaPublishMethod()
        {
            var broadcastProducer = new BroadcastProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => broadcastProducer.Publish(new DynamicDocument()));
            Assert.ThrowsAsync<ArgumentException>(() => broadcastProducer.PublishAsync(new DynamicDocument()));
        }

        [Test]
        public void TaskProducerThrowsExceptionIfDynamicDocumentSendViaPublishMethod()
        {
            var taskProducer = new TaskProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);

            Assert.Throws<ArgumentException>(() => taskProducer.Publish(new DynamicDocument()));
            Assert.ThrowsAsync<ArgumentException>(() => taskProducer.PublishAsync(new DynamicDocument()));
        }
    }
}