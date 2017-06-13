using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    [QueueName("StringConsumerWithAttributeTest")]
    public class StringTaskConsumerWithAttribute : TaskConsumerBase<string>
    {
        private readonly CountdownEvent _completeEvent;

        private readonly List<string> _messages;

        public StringTaskConsumerWithAttribute(List<string> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        protected override Task Consume(Message<string> message)
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