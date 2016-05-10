using System;
using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class Producer : IProducer
    {
        private const string QueueKey = "test_queue";

        public Producer(RabbitMqManager manager)
        {
            _channel = manager.GetConnection().CreateModel();
            manager.GetQueue(QueueKey);
        }

        private IModel _channel;

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", "test_queue", null, body);

            Console.WriteLine($"Produce: {message}");
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
            }
        }
    }
}