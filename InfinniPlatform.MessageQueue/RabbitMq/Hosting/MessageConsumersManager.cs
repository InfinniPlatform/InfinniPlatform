using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    internal sealed class MessageConsumersManager : ApplicationEventHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="consumers">Потребители сообщений.</param>
        /// <param name="manager">Мэнеджер соединения с RabbitMQ.</param>
        /// <param name="messageSerializer">Сериализатор сообщений.</param>
        /// <param name="log"></param>
        public MessageConsumersManager(IConsumer[] consumers,
                                       RabbitMqManager manager,
                                       IMessageSerializer messageSerializer,
                                       ILog log)
        {
            _manager = manager;
            _consumers = consumers;
            _messageSerializer = messageSerializer;
            _log = log;
        }

        private readonly IConsumer[] _consumers;
        private readonly ILog _log;
        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;

        public override void OnAfterStart()
        {
            var direct = _consumers.Where(consumer => consumer is ITaskConsumer);
            var fanout = _consumers.Where(consumer => consumer is IBroadcastConsumer);

            InitializeDirectConsumers(direct);
            InitializeFanoutConsumers(fanout);
        }

        private void InitializeDirectConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var channel = _manager.GetChannel();
                var channelKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                _manager.DeclareTaskQueue(channelKey);

                InitializeConsumer(channelKey, channel, consumer);
            }
        }

        private void InitializeFanoutConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var channel = _manager.GetChannel();
                var channelKey = _manager.DeclareFanoutQueue();

                InitializeConsumer(channelKey, channel, consumer);
            }
        }

        private void InitializeConsumer(string channelKey, IModel channel, IConsumer consumer)
        {
            if (channelKey == null)
            {
                throw new ArgumentException("Не указан ключ очереди.");
            }

            var eventingConsumer = new EventingBasicConsumer(channel);
            eventingConsumer.Received += (o, e) =>
                                         {
                                             var messageType = typeof(Message<>).MakeGenericType(consumer.MessageType);

                                             var message = _messageSerializer.BytesToMessage(e.Body, messageType);

                                             Task.Run(async () =>
                                                      {
                                                          _log.Info($"{DateTime.Now.Millisecond} Consumed by {consumer.GetType().Name}.");
                                                          Console.WriteLine($"{DateTime.Now.Millisecond} Consumed by {consumer.GetType().Name}.");
                                                          await consumer.Consume(message);
                                                      })
                                                 .ContinueWith(task =>
                                                               {
                                                                   if (!task.IsFaulted)
                                                                   {
                                                                       _log.Info($"{DateTime.Now.Millisecond} Acked by {consumer.GetType().Name}.");
                                                                       Console.WriteLine($"{DateTime.Now.Millisecond} Acked by {consumer.GetType().Name}.");
                                                                       channel.BasicAck(e.DeliveryTag, false);
                                                                   }
                                                                   else
                                                                   {
                                                                       Console.WriteLine(task.Exception);
                                                                   }
                                                               });
                                         };

            channel.BasicConsume(channelKey, false, eventingConsumer);
        }
    }
}