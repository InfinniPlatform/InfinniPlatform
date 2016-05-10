using System;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class EventingConsumer : IEventingConsumer
    {
        private const string QueueKey = "test_queue";

        public EventingConsumer(RabbitMqManager manager)
        {
            _channel = manager.GetConnection().CreateModel();
            manager.GetQueue(QueueKey);
            _channel.BasicQos(0, 1, false);

            _eventingConsumer = new EventingBasicConsumer(_channel);
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
            _channel.BasicConsume(QueueKey, true, _eventingConsumer);
        }
    }
}