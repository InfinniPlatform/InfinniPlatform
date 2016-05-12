using System;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class ProducerBase : IProducer
    {
        public ProducerBase(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _messageSerializer = messageSerializer;
        }

        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public void Produce(string queueName, IMessage message)
        {
            var messageToBytes = _messageSerializer.MessageToBytes(message);

            const string channelKey = nameof(ProducerBase);

            var channel = _manager.GetChannel(channelKey);
            _manager.DeclareQueue(queueName, channelKey);
            channel.BasicPublish("", queueName, null, messageToBytes);

            Console.WriteLine($"{channelKey}: {message.GetBody()}");
        }
    }
}