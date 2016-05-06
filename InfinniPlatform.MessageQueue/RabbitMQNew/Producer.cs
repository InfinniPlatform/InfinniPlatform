using System;
using System.Text;
using System.Threading;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class Producer : IProducer
    {
        public Producer(RabbitMqConnection connection)
        {
            _channel = connection.GetConnection().CreateModel();
            _channel.QueueDeclare("test_queue", false, false, false, null);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _properties = properties;
        }

        private readonly IBasicProperties _properties;

        private IModel _channel;

        public void Produce(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", "test_queue", _properties, body);

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} [x] Sent {message}");
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