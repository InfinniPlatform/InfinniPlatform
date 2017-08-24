using System.Threading.Tasks;
using InfinniPlatform.Dynamic;
using InfinniPlatform.MessageQueue.Management;

namespace InfinniPlatform.MessageQueue
{
    internal class BroadcastProducer : IBroadcastProducer
    {
        private readonly IBasicPropertiesProvider _basicPropertiesProvider;


        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public BroadcastProducer(RabbitMqManager manager,
                                 IMessageSerializer messageSerializer,
                                 IBasicPropertiesProvider basicPropertiesProvider)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _basicPropertiesProvider = basicPropertiesProvider;
        }


        public void Publish<T>(T messageBody, string queueName = null)
        {
            MessageQueueHelper.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicDocument messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            MessageQueueHelper.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        public Task PublishDynamicAsync(DynamicDocument messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        private void BasicPublish<T>(T messageBody, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);
            var routingKey = queueName ?? MessageQueueHelper.GetProducerQueueName(messageBody);

            using (var channel = _manager.GetChannel())
            {
                var basicProperties = _basicPropertiesProvider.Get();

                channel?.BasicPublish(_manager.BroadcastExchangeName, routingKey, true, basicProperties, messageToBytes);
            }
        }
    }
}