using System.Collections.Generic;
using System.Threading;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    public class DynamicWrapperConsumer : ConsumerBase<DynamicWrapper>
    {
        public DynamicWrapperConsumer(List<DynamicWrapper> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;
        private readonly List<DynamicWrapper> _messages;

        protected override void Consume(Message<DynamicWrapper> message)
        {
            _messages.Add(message.Body);
            _completeEvent.Signal();
        }
    }
}