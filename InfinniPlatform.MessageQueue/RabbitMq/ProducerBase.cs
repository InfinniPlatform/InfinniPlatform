using System;

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

            var channel = _manager.GetChannel(queueName);
            _manager.GetQueue(queueName);
            channel.BasicPublish("", queueName, null, messageToBytes);

            Console.WriteLine($"{nameof(ProducerBase)}: {message.GetBody()}");
        }
    }
}