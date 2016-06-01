using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.Tests.IntegrationTests.TestConsumers
{
    [QueueName("TestMessageWithAttributeTestQueue")]
    public class TestMessageWithAttributeTaskConsumer : TaskConsumerBase<TestMessageWithAttribute>
    {
        public TestMessageWithAttributeTaskConsumer(List<TestMessageWithAttribute> messages, CountdownEvent completeEvent)
        {
            _messages = messages;
            _completeEvent = completeEvent;
        }

        private readonly CountdownEvent _completeEvent;

        private readonly List<TestMessageWithAttribute> _messages;

        protected override Task Consume(Message<TestMessageWithAttribute> message)
        {
            return Task.Run(() =>
                            {
                                _messages.Add(message.Body);
                                _completeEvent.Signal();
                            });
        }
    }
}