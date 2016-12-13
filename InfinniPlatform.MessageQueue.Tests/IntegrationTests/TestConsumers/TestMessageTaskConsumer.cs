using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class TestMessageTaskConsumer : TaskConsumerBase<TestMessage>
    {
        public TestMessageTaskConsumer(List<TestMessage> messages, CountdownEvent completeEvent)
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

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}