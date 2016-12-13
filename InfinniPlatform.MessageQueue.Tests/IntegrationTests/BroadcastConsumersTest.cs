using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers;

using NUnit.Framework;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests
{
    [TestFixture]
    [Category(TestCategories.IntegrationTest)]
    public class BroadcastConsumersTest : RabbitMqTestBase
    {
        [Test]
        public async Task BroadcastConsumerRecieveAllMessages()
        {
            const int messageCount = 5;

            var assertMessages = new List<TestMessage>();

            for (var i = 1; i <= messageCount; i++)
            {
                assertMessages.Add(new TestMessage(i.ToString(), i, new DateTime(i, i, i)));
            }

            var actualMessages = new List<TestMessage>();
            var completeEvent = new CountdownEvent(messageCount);

            IBroadcastConsumer[] broadcastConsumers =
            {
                new TestMessageBroadcastConsumer(actualMessages, completeEvent)
            };

            RegisterConsumers(null, broadcastConsumers);

            var producerBase = new BroadcastProducer(RabbitMqManager, MessageSerializer, BasicPropertiesProvider);
            foreach (var message in assertMessages)
            {
                await producerBase.PublishAsync(message);
            }

            const int timeout = 5000;

            Assert.IsTrue(completeEvent.Wait(timeout), $"Failed finish message consuming in {timeout} ms for {nameof(TestMessageBroadcastConsumer)}.");
            CollectionAssert.AreEquivalent(assertMessages, actualMessages);
        }
    }
}