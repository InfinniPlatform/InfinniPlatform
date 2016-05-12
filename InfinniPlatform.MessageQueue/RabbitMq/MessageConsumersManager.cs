using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    internal sealed class MessageConsumersManager : ApplicationEventHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="manager">Мэнеджер соединения с RabbitMQ.</param>
        /// <param name="consumers">Потребители сообщений, зарегистрированные в IoC.</param>
        /// <param name="messageSerializer">Сериализатор сообщений.</param>
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
                var channelKey = consumer.GetType().ToString();
                var channel = _manager.GetChannel(channelKey);

                _manager.DeclareQueue(consumer.QueueName, channelKey);

                var eventingConsumer = new EventingBasicConsumer(channel);

                eventingConsumer.Received += (o, e) =>
                                             {
                                                 var messageType = typeof(Message<>).MakeGenericType(consumer.MessageType);
                                                 var message = _messageSerializer.BytesToMessage(e.Body, messageType);

                                                 consumer.Consume(message);

                                                 channel.BasicAck(e.DeliveryTag, false);
                                             };

                channel.BasicConsume(consumer.QueueName, false, eventingConsumer);
            }
        }
    }
}