using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    public class BaseNamedQueueDynamicWrapperTaskConsumer : TaskConsumerBase<DynamicWrapper>
    {
        public BaseNamedQueueDynamicWrapperTaskConsumer(List<DynamicWrapper> messages,
                                                        CountdownEvent completeEvent,
                                                        int taskWorkTime = 0)
        {
            _messages = messages;
            _completeEvent = completeEvent;
            _taskWorkTime = taskWorkTime;
        }

        private readonly CountdownEvent _completeEvent;
        private readonly List<DynamicWrapper> _messages;
        private readonly int _taskWorkTime;

        protected override Task Consume(Message<DynamicWrapper> message)
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
    public class Queue1DynamicWrapperTaskConsumer : BaseNamedQueueDynamicWrapperTaskConsumer
    {
        public Queue1DynamicWrapperTaskConsumer(List<DynamicWrapper> messages,
                                                CountdownEvent completeEvent,
                                                int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }


    [QueueName("Queue2")]
    public class Queue2DynamicWrapperTaskConsumer : BaseNamedQueueDynamicWrapperTaskConsumer
    {
        public Queue2DynamicWrapperTaskConsumer(List<DynamicWrapper> messages,
                                                CountdownEvent completeEvent,
                                                int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }


    [QueueName("Queue3")]
    public class Queue3DynamicWrapperTaskConsumer : BaseNamedQueueDynamicWrapperTaskConsumer
    {
        public Queue3DynamicWrapperTaskConsumer(List<DynamicWrapper> messages,
                                                CountdownEvent completeEvent,
                                                int taskWorkTime = 0)
            : base(messages, completeEvent, taskWorkTime)
        {
        }
    }
}