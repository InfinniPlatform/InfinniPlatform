using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class OnDemandConsumer : IOnDemandConsumer
    {
        public OnDemandConsumer(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public Task<IMessage> Consume<T>(string queueName = null)
        {
            using (var channel = _manager.GetChannel())
            {
                if (queueName == null)
                {
                    queueName = QueueNamingConventions.GetBasicConsumerQueueName(typeof(T));
                }

                _manager.DeclareTaskQueue(queueName);

                var result = channel.BasicGet(queueName, false);

                if (result == null)
                {
                    return Task.FromResult<IMessage>(null);
                }

                var message = _messageSerializer.BytesToMessage<T>(result);

                channel.BasicAck(result.DeliveryTag, false);

                return Task.FromResult(message);
            }
        }
    }
}