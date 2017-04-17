using System.Threading.Tasks;

using InfinniPlatform.Core.Abstractions.Dynamic;
using InfinniPlatform.MessageQueue.Abstractions.Producers;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMQ.Serialization;

namespace InfinniPlatform.MessageQueue.RabbitMQ
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

        private readonly IBasicPropertiesProvider _basicPropertiesProvider;

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            BasicPublish(messageBody, queueName);
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            BasicPublish(messageBody, queueName);
        }

        public async Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            await Task.Run(() => { BasicPublish(messageBody, queueName); });
        }

        public async Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            await Task.Run(() => { BasicPublish(messageBody, queueName); });
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