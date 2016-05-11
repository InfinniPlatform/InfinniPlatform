using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class MessageConsumersManager : ApplicationEventHandler
    {
        public MessageConsumersManager(RabbitMqManager manager,
                                       IConsumer[] consumers,
                                       IMessageSerializer messageSerializer)
        {
            _manager = manager;
            _consumers = consumers;
            _messageSerializer = messageSerializer;
        }

        private readonly IConsumer[] _consumers;
        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public override void OnAfterStart()
        {
            RegisterConsumers();
        }

        private void RegisterConsumers()
        {
            foreach (var consumer in _consumers)
            {
                var channel = _manager.GetChannel(consumer.QueueName);

                _manager.GetQueue(consumer.QueueName);

                var eventingConsumer = new EventingBasicConsumer(channel);

                eventingConsumer.Received += (o, e) =>
                                             {
                                                 var messageType = typeof(Message<>).MakeGenericType(consumer.MessageType);
                                                 var message = _messageSerializer.BytesToMessage(e.Body, messageType);
                                                 consumer.Consume(message);
                                             };

                channel.BasicConsume(consumer.QueueName, true, eventingConsumer);
            }
        }
    }
}