using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class DynamicWrapperTaskConsumer : TaskConsumerBase<DynamicWrapper>
    {
        public DynamicWrapperTaskConsumer(List<DynamicWrapper> messages,
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

        protected override void Consume(Message<DynamicWrapper> message)
        {
            _messages.Add(message.Body);
            _completeEvent.Signal();
            Thread.Sleep(_taskWorkTime);
        }

        protected override Task ConsumeAsync(Message<DynamicWrapper> message)
        {
            throw new System.NotImplementedException();
        }
    }
}