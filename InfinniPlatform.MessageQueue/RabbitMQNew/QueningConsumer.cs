using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        public BasicConsumer(RabbitMqConnection connection)
        {
            _channel = connection.GetConnection().CreateModel();
            _channel.QueueDeclare("test_queue", false, false, false, null);
            _channel.BasicQos(0, 1, false);
        }


        private IModel _channel;

        public string Get()
        {
            var message = _channel.BasicGet("test_queue", false);

            return message == null ? null : Encoding.UTF8.GetString(message.Body);
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