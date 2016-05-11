using System;
using System.Text;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class Producer : IProducer
    {
        public Producer(RabbitMqManager manager)
        {
            _manager = manager;
        }

        private readonly RabbitMqManager _manager;

        public void Produce(string queueName, byte[] message)
        {
            var channel = _manager.GetChannel(queueName);
            _manager.GetQueue(queueName);
            channel.BasicPublish("", queueName, null, message);

            Console.WriteLine($"{nameof(Producer)}: {Encoding.UTF8.GetString(message)}");
        }
    }
}