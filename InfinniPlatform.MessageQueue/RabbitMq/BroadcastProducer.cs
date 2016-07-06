using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Queues.Producers;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal class BroadcastProducer : IBroadcastProducer
    {
        public BroadcastProducer(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

            using (var channel = _manager.GetChannel())
            {
                channel.BasicPublish(_manager.BroadcastExchangeName, queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody), null, messageToBytes);
            }
        }

        public void PublishDynamic(DynamicWrapper messageBody, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

            using (var channel = _manager.GetChannel())
            {
                channel.BasicPublish(_manager.BroadcastExchangeName, queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody), null, messageToBytes);
            }
        }

        public async Task PublishAsync<T>(T messageBody, string queueName = null)
        {
            Helpers.CheckTypeRestrictions<T>();

            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(messageBody);

                               using (var channel = _manager.GetChannel())
                               {
                                   channel.BasicPublish(_manager.BroadcastExchangeName, queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody), null, messageToBytes);
                               }
                           });
        }

        public async Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName)
        {
            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(messageBody);
                               using (var channel = _manager.GetChannel())
                               {
                                   channel.BasicPublish(_manager.BroadcastExchangeName, queueName ?? QueueNamingConventions.GetProducerQueueName(messageBody), null, messageToBytes);
                               }
                           });
        }
    }
}