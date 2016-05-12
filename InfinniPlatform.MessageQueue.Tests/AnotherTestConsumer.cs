using System;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests
{
    public class AnotherTestConsumer : ConsumerBase<TestMessage>
    {
        public AnotherTestConsumer() : base("test_queue")
        {
        }

        protected override void Consume(Message<TestMessage> message)
        {
            Console.WriteLine($"{nameof(AnotherTestConsumer)}: {message.Body}");
        }
    }
}