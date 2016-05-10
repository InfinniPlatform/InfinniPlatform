using System;
using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        private const string QueueKey = "test_queue";

        public BasicConsumer(RabbitMqManager manager)
        {
            _channel = manager.GetChannel(QueueKey);
            manager.GetQueue(QueueKey);
        }

        private IModel _channel;

        public string Get()
        {
            var message = _channel.BasicGet(QueueKey, true);
            var serializedMessage = message == null
                                        ? null
                                        : Encoding.UTF8.GetString(message.Body);

            Console.WriteLine($"BasicConsumer: {serializedMessage}");

            return serializedMessage;
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