using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.Sdk.Queues;

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

        protected override async Task Consume(Message<TestMessageWithAttribute> message)
        {
            await Task.Run(() =>
                           {
                               _messages.Add(message.Body);
                               _completeEvent.Signal();
                           });
        }
    }
}