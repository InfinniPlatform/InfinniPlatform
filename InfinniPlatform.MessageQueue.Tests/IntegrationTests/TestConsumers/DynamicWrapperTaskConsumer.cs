using System;
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

        protected override async Task Consume(Message<DynamicWrapper> message)
        {
            await Task.Run(async () =>
                                 {
                                     _messages.Add(message.Body);
                                     _completeEvent.Signal();
                                     Console.WriteLine($"{DateTime.Now.Millisecond} Wait.");
                                     await Task.Delay(_taskWorkTime);
                                     Console.WriteLine($"{DateTime.Now.Millisecond} Wait complete.");
                                 });
        }
    }
}