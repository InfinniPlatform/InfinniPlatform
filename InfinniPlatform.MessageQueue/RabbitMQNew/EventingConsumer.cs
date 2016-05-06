using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class EventingConsumer : IEventingConsumer
    {
        public EventingConsumer(RabbitMqConnection connection)
        {
            _channel = connection.GetConnection().CreateModel();
            _channel.QueueDeclare("test_queue", false, false, false, null);
            _channel.BasicQos(0, 1, false);

            _eventingConsumer = new EventingBasicConsumer(_channel);

            _channel.BasicConsume("test_queue", false, _eventingConsumer);
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

        public void AddRecievedEvent(EventHandler<BasicDeliverEventArgs> eventingConsumerOnReceived)
        {
            _eventingConsumer.Received += eventingConsumerOnReceived;
        }
    }
}