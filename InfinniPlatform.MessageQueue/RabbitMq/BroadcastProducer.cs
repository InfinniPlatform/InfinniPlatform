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

        public void Publish<T>(T message, string queueName = null)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(message);
            var channel = _manager.GetChannel();

            channel.BasicPublish(_manager.GetExchangeNameByType(Defaults.Exchange.Type.Fanout), "", null, messageToBytes);
        }

        public void PublishDynamic(DynamicWrapper message, string queueName)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(message);
            var channel = _manager.GetChannel();

            channel.BasicPublish(_manager.GetExchangeNameByType(Defaults.Exchange.Type.Fanout), "", null, messageToBytes);
        }

        public async Task PublishAsync<T>(T message, string queueName = null)
        {
            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(message);
                               var channel = _manager.GetChannel();

                               channel.BasicPublish(_manager.GetExchangeNameByType(Defaults.Exchange.Type.Fanout), "", null, messageToBytes);
                           });
        }

        public async Task PublishDynamicAsync(DynamicWrapper message, string queueName)
        {
            await Task.Run(() =>
                           {
                               var messageToBytes = _messageSerializer.MessageToBytes(message);
                               var channel = _manager.GetChannel();

                               channel.BasicPublish(_manager.GetExchangeNameByType(Defaults.Exchange.Type.Fanout), "", null, messageToBytes);
                           });
        }
    }
}