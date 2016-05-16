using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        public BasicConsumer(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public IMessage Consume<T>() where T : class
        {
            var channel = _manager.GetChannel();

            var queueName = QueueNamingConventions.GetBasicConsumerQueueName(typeof(T));

            _manager.DeclareTaskQueue(queueName);

            var getResult = channel.BasicGet(queueName, false);

            if (getResult == null)
            {
                return null;
            }

            var message = _messageSerializer.BytesToMessage(getResult.Body, typeof(Message<T>));

            channel.BasicAck(getResult.DeliveryTag, false);

            return message;
        }
    }
}