using System.Text;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class QueningConsumer : IQueningConsumer
    {
        public QueningConsumer(RabbitMqConnection connection)
        {
            _channel = connection.GetConnection().CreateModel();
            _channel.QueueDeclare("test_queue", false, false, false, null);
            _channel.BasicQos(0, 1, false);

            _queueingConsumer = new QueueingBasicConsumer(_channel);
        }

        private readonly QueueingBasicConsumer _queueingConsumer;


        private IModel _channel;

        public string Consume()
        {
            BasicDeliverEventArgs args;
            _channel.BasicConsume("test_queue", false, _queueingConsumer);
            _queueingConsumer.Queue.Dequeue(100, out args);

            return args == null ? null : Encoding.UTF8.GetString(args.Body);
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