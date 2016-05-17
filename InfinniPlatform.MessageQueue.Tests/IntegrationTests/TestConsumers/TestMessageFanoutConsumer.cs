using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class TestMessageFanoutConsumer : FanoutConsumerBase<TestMessage>
    {
        public TestMessageFanoutConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessage> _messages;

        protected override void Consume(Message<TestMessage> message)
        {
            _messages.Add(message.Body);
            _completeEvent.Signal();
        }
    }
}