using System;
using System.Text;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        public BasicConsumer(RabbitMqManager manager)
        {
            _channel = manager.GetChannel(QueueName);
            manager.GetQueue(QueueName);
        }

        private readonly IModel _channel;

        public string QueueName => "test_queue";

        public string Consume()
        {
            var message = _channel.BasicGet(QueueName, true);
            var serializedMessage = message == null
                                        ? null
                                        : Encoding.UTF8.GetString(message.Body);

            Console.WriteLine($"{nameof(BasicConsumer)}: {serializedMessage}");

            return serializedMessage;
        }
    }
}