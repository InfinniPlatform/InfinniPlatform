using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public class QueningConsumer : IQueningConsumer
    {
        public QueningConsumer()
        {
            var factory = new ConnectionFactory
                          {
                              HostName = "localhost"
                          };

            _connection = factory.CreateConnection();

            _channel = _connection.CreateModel();

            _channel.QueueDeclare("task_queue", true, false, false, null);
            _channel.BasicQos(0, 1, false);
            _queueingConsumer = new QueueingBasicConsumer(_channel);
        }

        private readonly QueueingBasicConsumer _queueingConsumer;


        private IModel _channel;
        private IConnection _connection;

        public string Consume()
        {
            _channel.BasicConsume("task_queue", false, _queueingConsumer);
            var args = _queueingConsumer.Queue.Dequeue();

            return args == null ? null : Encoding.UTF8.GetString(args.Body);
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