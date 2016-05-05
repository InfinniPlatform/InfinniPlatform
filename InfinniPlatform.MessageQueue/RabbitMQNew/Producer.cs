using System;
using System.Text;
using System.Threading;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public class Producer : IProducer
    {
        public Producer()
        {
            var factory = new ConnectionFactory
                          {
                              HostName = "localhost"
                          };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();
            _channel.QueueDeclare("task_queue", true, false, false, null);
            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true;

            _properties = properties;
        }

        private readonly IBasicProperties _properties;

        private IModel _channel;
        private IConnection _connection;

        public void Produce(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish("", "task_queue", _properties, body);

            Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} [x] Sent {message}");
        }

        public void Dispose()
        {
            if (_channel != null)
            {
                _channel.Close();
                _channel = null;
                if (_connection != null)
                {
                    _connection.Close();
                    _connection = null;
                }
            }
        }
    }
}