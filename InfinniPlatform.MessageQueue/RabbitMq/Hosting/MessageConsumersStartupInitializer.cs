using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.MessageQueue.RabbitMq.Connection;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
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
            try
            {
                _log.Info("Starting consumer initialization.");

                var taskConsumers = _taskConsumers.ToArray();
                var broadcastConsumers = _broadcastConsumers.ToArray();

                InitializeTaskConsumers(taskConsumers);
                _log.Info($"Initialization of {taskConsumers.Length} task consumers successfully completed.");
                InitializeBroadcastConsumers(broadcastConsumers);
                _log.Info($"Initialization of {broadcastConsumers.Length} broadcast consumers successfully completed.");
            }
            catch (Exception e)
            {
                _log.Error(Resources.UnableToInitializeConsumers, exception: e);
            }
        }

        private void InitializeTaskConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                _log.Debug($"Initialization of {consumer.GetType().Name} task consumer started.");

                var channel = _manager.GetChannel();
                var queueName = QueueNamingConventions.GetConsumerQueueName(consumer);
                _manager.DeclareTaskQueue(queueName);

                InitializeConsumer(queueName, channel, consumer);

                _log.Debug($"Initialization of {consumer.GetType().Name} task consumer successfully completed.");
            }
        }

        private void InitializeBroadcastConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                _log.Debug($"Initialization of {consumer.GetType().Name} broadcast consumer started.");

                var channel = _manager.GetChannel();
                var routingKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                InitializeConsumer(queueName, channel, consumer);

                _log.Debug($"Initialization of {consumer.GetType().Name} broadcast consumer successfully completed.");
            }
        }

        private void InitializeConsumer(string queueName, IModel channel, IConsumer consumer)
        {
            if (queueName == null)
            {
                throw new ArgumentException(Resources.UnableToGetQueueName);
            }

            var eventingConsumer = new EventingBasicConsumer(channel);
            eventingConsumer.Received += (o, args) =>
                                         {
                                             var startDate = DateTime.Now;
                                             IMessage message;
                                             string typeName;

                                             try
                                             {
                                                 message = _messageSerializer.BytesToMessage(args.Body, consumer.MessageType);
                                                 typeName = consumer.GetType().Name;
                                             }
                                             catch (Exception e)
                                             {
                                                 _log.Error(e);
                                                 return;
                                             }

                                             Task.Run(async () =>
                                                            {
                                                                startDate = DateTime.Now;

                                                                _log.Debug(string.Format(Resources.ConsumeStart, args.DeliveryTag, typeName));

                                                                await consumer.Consume(message);

                                                                _log.Debug(string.Format(Resources.ConsumeSuccess, args.DeliveryTag, typeName));
                                                            })
                                                 .ContinueWith(task =>
                                                               {
                                                                   if (task.IsFaulted)
                                                                   {
                                                                       _log.Error(task.Exception);
                                                                       _performanceLog.Log(typeName, startDate, task.Exception);
                                                                   }
                                                                   else
                                                                   {
                                                                       _performanceLog.Log(typeName, startDate);
                                                                       _log.Debug(string.Format(Resources.AckStart, args.DeliveryTag, typeName));

                                                                       channel.BasicAck(args.DeliveryTag, false);

                                                                       _log.Debug(string.Format(Resources.AckSuccess, args.DeliveryTag, typeName));
                                                                   }
                                                               });
                                         };

            channel.BasicConsume(queueName, false, eventingConsumer);
        }
    }
}