using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

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

        protected override Task Consume(Message<DynamicWrapper> message)
        {
            return Task.Run(async () =>
                                  {
                                      _messages.Add(message.Body);
                                      _completeEvent.Signal();
                                      await Task.Delay(_taskWorkTime);
                                  });
        }
    }
}