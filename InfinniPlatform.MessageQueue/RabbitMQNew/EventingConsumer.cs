using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public class EventingConsumer : IEventingConsumer
    {
        public EventingConsumer()
        {
            var factory = new ConnectionFactory
                          {
                              HostName = "localhost"
                          };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare("task_queue", true, false, false, null);
            _channel.BasicQos(0, 1, false);

            _eventingConsumer = new EventingBasicConsumer(_channel);

            _channel.BasicConsume("task_queue", false, _eventingConsumer);
        }

        private readonly EventingBasicConsumer _eventingConsumer;


        private IModel _channel;
        private IConnection _connection;

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

        public void AddRecievedEvend(EventHandler<BasicDeliverEventArgs> eventingConsumerOnReceived)
        {
            _eventingConsumer.Received += eventingConsumerOnReceived;
        }
    }
}