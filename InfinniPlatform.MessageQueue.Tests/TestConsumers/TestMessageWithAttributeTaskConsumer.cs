using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    [QueueName("TestMessageWithAttributeTestQueue")]
    public class TestMessageWithAttributeTaskConsumer : TaskConsumerBase<TestMessageWithAttribute>
    {
        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessageWithAttribute> _messages;

        public TestMessageWithAttributeTaskConsumer(List<TestMessageWithAttribute> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        protected override Task Consume(Message<TestMessageWithAttribute> message)
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