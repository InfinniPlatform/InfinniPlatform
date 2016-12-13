using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.MessageQueue.Contract.Consumers;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;

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
                    queueName = QueueNamingConventions.GetQueueName(typeof(T));
                }

                var declareQueueName = _manager.DeclareTaskQueue(queueName);

                var result = channel?.BasicGet(declareQueueName, false);

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