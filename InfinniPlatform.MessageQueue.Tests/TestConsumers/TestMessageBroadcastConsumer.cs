using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    public class TestMessageBroadcastConsumer : BroadcastConsumerBase<TestMessage>
    {
        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessage> _messages;

        public TestMessageBroadcastConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        protected override Task Consume(Message<TestMessage> message)
        {
            return Task.Run(() =>
            {
                _messages.Add(message.Body);
                _completeEvent.Signal();
            });
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }


    [QueueName("NamedQueueTestMessageBroadcast")]
    public class NamedQueueTestMessageBroadcastConsumer : BroadcastConsumerBase<TestMessage>
    {
        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessage> _messages;

        public NamedQueueTestMessageBroadcastConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        protected override Task Consume(Message<TestMessage> message)
        {
            return Task.Run(() =>
            {
                _messages.Add(message.Body);
                _completeEvent.Signal();
            });
        }
    }
}