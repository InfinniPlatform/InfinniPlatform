using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues.Consumers;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    internal sealed class MessageConsumersStartupInitializer : ApplicationEventHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="taskConsumers">Потребители сообщений очереди задач.</param>
        /// ///
        /// <param name="broadcastConsumers">Потребители сообщений широковещательной очереди.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="messageSerializer">Сериализатор сообщений.</param>
        /// <param name="log">Лог.</param>
        /// <param name="performanceLog">Лог производительности.</param>
        public MessageConsumersStartupInitializer(IEnumerable<ITaskConsumer> taskConsumers,
                                       IEnumerable<IBroadcastConsumer> broadcastConsumers,
                                       RabbitMqManager manager,
                                       IMessageSerializer messageSerializer,
                                       ILog log,
                                       IPerformanceLog performanceLog)
        {
            _taskConsumers = taskConsumers;
            _broadcastConsumers = broadcastConsumers;
            _manager = manager;
            _messageSerializer = messageSerializer;
            _log = log;
            _performanceLog = performanceLog;
        }

        private readonly IEnumerable<IBroadcastConsumer> _broadcastConsumers;
        private readonly ILog _log;
        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IPerformanceLog _performanceLog;
        private readonly IEnumerable<ITaskConsumer> _taskConsumers;

        public override void OnAfterStart()
        {
            InitializeTaskConsumers(_taskConsumers);
            InitializeBroadcastConsumers(_broadcastConsumers);
        }

        private void InitializeTaskConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var channel = _manager.GetChannel();
                var channelKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                _manager.DeclareTaskQueue(channelKey);

                InitializeConsumer(channelKey, channel, consumer);
            }
        }

        private void InitializeBroadcastConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var channel = _manager.GetChannel();
                var routingKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                InitializeConsumer(queueName, channel, consumer);
            }
        }

        private void InitializeConsumer(string queueName, IModel channel, IConsumer consumer)
        {
            if (queueName == null)
            {
                throw new ArgumentException("Не указано имя очереди.");
            }

            var eventingConsumer = new EventingBasicConsumer(channel);
            eventingConsumer.Received += (o, e) =>
                                         {
                                             var message = _messageSerializer.BytesToMessage(e.Body, consumer.MessageType);

                                             var startDate = DateTime.Now;

                                             var name = consumer.GetType().Name;

                                             Task.Run(async () =>
                                                            {
                                                                startDate = DateTime.Now;
                                                                _log.Debug($"Started consume by {name}.");

                                                                await consumer.Consume(message);

                                                                _log.Debug($"Finished consume by {name}.");
                                                            })
                                                 .ContinueWith(task =>
                                                               {
                                                                   if (task.IsFaulted)
                                                                   {
                                                                       _log.Error(task.Exception);
                                                                       _performanceLog.Log(name, startDate, task.Exception);
                                                                   }
                                                                   else
                                                                   {
                                                                       _performanceLog.Log(name, startDate);
                                                                       _log.Debug($"Started ack by {name}.");

                                                                       channel.BasicAck(e.DeliveryTag, false);

                                                                       _log.Debug($"Finished ack by {name}.");
                                                                   }
                                                               });
                                         };

            channel.BasicConsume(queueName, false, eventingConsumer);
        }
    }
}