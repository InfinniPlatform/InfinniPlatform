using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal class BroadcastProducerBase : IBroadcastProducer
    {
        public BroadcastProducerBase(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Publish(IMessage message)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(message);
            var channel = _manager.GetChannel();

            channel.BasicPublish(_manager.GetExchangeNameByType(Defaults.Exchange.Type.Fanout), "", null, messageToBytes);
        }
    }
}