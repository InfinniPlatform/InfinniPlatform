using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class TestMessageBroadcastConsumer : BroadcastConsumerBase<TestMessage>
    {
        public TestMessageBroadcastConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessage> _messages;

        protected override Task Consume(Message<TestMessage> message)
        {
            return Task.Run(() =>
                            {
                                _messages.Add(message.Body);
                                _completeEvent.Signal();
                            });
        }

        protected override Task<bool> OnError()
        {
            return Task.FromResult(false);
        }
    }


    [QueueName("NamedQueueTestMessageBroadcast")]
    public class NamedQueueTestMessageBroadcastConsumer : BroadcastConsumerBase<TestMessage>
    {
        public NamedQueueTestMessageBroadcastConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessage> _messages;

        protected override Task Consume(Message<TestMessage> message)
        {
            return Task.Run(() =>
                            {
                                _messages.Add(message.Body);
                                _completeEvent.Signal();
                            });
        }

        protected override Task<bool> OnError()
        {
            return Task.FromResult(false);
        }
    }
}