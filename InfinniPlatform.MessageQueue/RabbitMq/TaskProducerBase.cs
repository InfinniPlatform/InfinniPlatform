using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class TaskProducerBase : ITaskProducer
    {
        public TaskProducerBase(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish(IMessage message, string queueName = null)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(message);

            if (queueName == null)
            {
                queueName = QueueNamingConventions.GetProducerQueueName(message);
            }

            var channel = _manager.GetChannel();

            _manager.DeclareTaskQueue(queueName);

            channel.BasicPublish("", queueName, null, messageToBytes);
        }
    }
}