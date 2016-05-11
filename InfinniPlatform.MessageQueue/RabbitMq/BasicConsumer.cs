using System;

using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class BasicConsumer : IBasicConsumer
    {
        public BasicConsumer(RabbitMqManager manager, IMessageSerializer messageSerializer)
        {
            _messageSerializer = messageSerializer;
            _channel = manager.GetChannel(QueueName);
            manager.GetQueue(QueueName);
        }

        private readonly IModel _channel;
        private readonly IMessageSerializer _messageSerializer;

        public string QueueName => "test_queue";

        public IMessage Consume<T>() where T : class
        {
            var message = _channel.BasicGet(QueueName, true);

            var serializedMessage = _messageSerializer.BytesToMessage(message?.Body, typeof(Message<T>));

            Console.WriteLine($"{nameof(BasicConsumer)}: {serializedMessage.GetBody()}");

            return serializedMessage;
        }
    }
}