using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal class BroadcastProducer : IBroadcastProducer
    {
        public BroadcastProducer(RabbitMqManager manager,
                                 IMessageSerializer messageSerializer,
                                 IBasicPropertiesProvider basicPropertiesProvider)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
            _basicPropertiesProvider = basicPropertiesProvider;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IBasicPropertiesProvider _basicPropertiesProvider;

        public void Publish<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        public Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);

            return Task.CompletedTask;
        }

        private void BasicPublish<T>(T messageBody, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);
            var routingKey = queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody);

            using (var channel = _manager.GetChannel())
            {
                var basicProperties = _basicPropertiesProvider.Get();

                channel?.BasicPublish(_manager.BroadcastExchangeName, routingKey, true, basicProperties, messageToBytes);
            }
        }
    }
}