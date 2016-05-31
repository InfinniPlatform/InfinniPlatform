using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
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

        public void Publish<T>(T message, string queueName = null) where T : class
        {
            var innerMessage = new Message<T>(message);

            var messageToBytes = _messageSerializer.MessageToBytes(innerMessage);

            if (queueName == null)
            {
                queueName = QueueNamingConventions.GetProducerQueueName(innerMessage);
            }

            var channel = _manager.GetChannel();

            _manager.DeclareTaskQueue(queueName);

            channel.BasicPublish("", queueName, null, messageToBytes);
        }

        public void Publish(DynamicWrapper message, string queueName)
        {
            var innerMessage = new Message(message);

            var messageToBytes = _messageSerializer.MessageToBytes(innerMessage);

            if (queueName == null)
            {
                queueName = QueueNamingConventions.GetProducerQueueName(innerMessage);
            }

            var channel = _manager.GetChannel();

            _manager.DeclareTaskQueue(queueName);

            channel.BasicPublish("", queueName, null, messageToBytes);
        }
    }
}