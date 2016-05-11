using System;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class Producer : IProducer
    {
        public Producer(RabbitMqManager manager)
        {
            _channel = manager.GetChannel(QueueName);
            manager.GetQueue(QueueName);
        }

        private readonly IModel _channel;

        public string QueueName => "test_queue";

        public void Produce(byte[] message)
        {
            _channel.BasicPublish("", QueueName, null, message);

            Console.WriteLine($"{nameof(Producer)}: {message}");
        }
    }
}