using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    public class BaseNamedQueueDynamicDocumentTaskConsumer : TaskConsumerBase<DynamicDocument>
    {
        private readonly CountdownEvent _completeEvent;
        private readonly List<DynamicDocument> _messages;
        private readonly int _taskWorkTime;

        public BaseNamedQueueDynamicDocumentTaskConsumer(List<DynamicDocument> messages,
                                                         CountdownEvent completeEvent,
                                                         int taskWorkTime = 0)
        {
            _messages = messages;
            _completeEvent = completeEvent;
            _taskWorkTime = taskWorkTime;
        }

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


    [QueueName("Queue1")]
    public class Queue1DynamicDocumentTaskConsumer : BaseNamedQueueDynamicDocumentTaskConsumer
    {
        public Queue1DynamicDocumentTaskConsumer(List<DynamicDocument> messages,
                                                 CountdownEvent completeEvent,
                                                 int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }


    [QueueName("Queue2")]
    public class Queue2DynamicDocumentTaskConsumer : BaseNamedQueueDynamicDocumentTaskConsumer
    {
        public Queue2DynamicDocumentTaskConsumer(List<DynamicDocument> messages,
                                                 CountdownEvent completeEvent,
                                                 int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }


    [QueueName("Queue3")]
    public class Queue3DynamicDocumentTaskConsumer : BaseNamedQueueDynamicDocumentTaskConsumer
    {
        public Queue3DynamicDocumentTaskConsumer(List<DynamicDocument> messages,
                                                 CountdownEvent completeEvent,
                                                 int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }
}