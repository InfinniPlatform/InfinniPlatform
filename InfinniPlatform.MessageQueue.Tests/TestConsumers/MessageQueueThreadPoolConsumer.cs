using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace InfinniPlatform.MessageQueue.TestConsumers
{
    [QueueName("MessageQueueThreadPoolTest")]
    public class MessageQueueThreadPoolConsumer : TaskConsumerBase<string>
    {
        private readonly CountdownEvent _completeEvent;
        private readonly ConcurrentBag<string> _messages;
        private readonly int _workTime;

        public MessageQueueThreadPoolConsumer(ConcurrentBag<string> messages,
                                              CountdownEvent completeEvent,
                                              int workTime)
        {
            _messages = messages;
            _completeEvent = completeEvent;
            _workTime = workTime;
        }

        protected override async Task Consume(Message<string> message)
        {
            _messages.Add(message.Body);
            await Task.Delay(_workTime);
            _completeEvent.Signal();
        }
    }
}