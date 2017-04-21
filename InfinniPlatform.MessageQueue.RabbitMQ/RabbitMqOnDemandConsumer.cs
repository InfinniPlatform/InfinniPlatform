using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue
{
    internal class RabbitMqOnDemandConsumer : IOnDemandConsumer
    {
        public RabbitMqOnDemandConsumer(RabbitMqManager manager, IRabbitMqMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }


        private readonly RabbitMqManager _manager;
        private readonly IRabbitMqMessageSerializer _messageSerializer;


        public Task<IMessage> Consume<T>(string queueName = null)
        {
            using (var channel = _manager.GetChannel())
            {
                if (queueName == null)
                {
                    queueName = RabbitMqHelper.GetQueueName(typeof(T));
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