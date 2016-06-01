using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

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
    }
}