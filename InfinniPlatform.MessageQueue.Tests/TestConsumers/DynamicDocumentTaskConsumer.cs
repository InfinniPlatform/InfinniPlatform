using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    public class DynamicDocumentTaskConsumer : TaskConsumerBase<DynamicDocument>
    {
        public DynamicDocumentTaskConsumer(List<DynamicDocument> messages,
                                          CountdownEvent completeEvent,
                                          int taskWorkTime = 0)
        {
            _messages = messages;
            _completeEvent = completeEvent;
            _taskWorkTime = taskWorkTime;
        }

        private readonly CountdownEvent _completeEvent;
        private readonly List<DynamicDocument> _messages;
        private readonly int _taskWorkTime;

        protected override Task Consume(Message<DynamicDocument> message)
        {
            return Task.Run(async () =>
                            {
                                _messages.Add(message.Body);
                                _completeEvent.Signal();
                                await Task.Delay(_taskWorkTime);
                            });
        }

        protected override Task<bool> OnError(Exception exception)
        {
            return Task.FromResult(false);
        }
    }
}