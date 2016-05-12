using System;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests
{
    public class TestConsumer : ConsumerBase<TestMessage>
    {
        public TestConsumer() : base("test_queue")
        {
        }

        protected override void Consume(Message<TestMessage> message)
        {
            Console.WriteLine($"{nameof(TestConsumer)}: {message.Body}");
        }
    }
}