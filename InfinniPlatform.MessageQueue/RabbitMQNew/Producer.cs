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
            _channel.BasicPublish("", QueueKey, null, Encoding.UTF8.GetBytes(DateTime.Now.ToString("U")));
        }

        //private readonly IBasicProperties _properties;

        private IModel _channel;

        public void Publish(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            var properties = _channel.CreateBasicProperties();

            _channel.BasicPublish("", "test_queue", properties, body);

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