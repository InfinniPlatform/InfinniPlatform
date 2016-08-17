using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMq.Serialization;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    internal sealed class MessageConsumersStartupInitializer : AppEventHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="consumerSource">Источники потребителей сообщений.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="messageSerializer">Сериализатор сообщений.</param>
        /// <param name="performanceLog">Лог производительности.</param>
        /// <param name="log">Лог.</param>
        public MessageConsumersStartupInitializer(IEnumerable<IMessageConsumerSource> consumerSource,
                                                  RabbitMqManager manager,
                                                  IMessageSerializer messageSerializer,
                                                  IPerformanceLog performanceLog,
                                                  ILog log)
        {
            _consumerSource = consumerSource;
            _manager = manager;
            _messageSerializer = messageSerializer;
            _performanceLog = performanceLog;
            _log = log;
        }


        private readonly IEnumerable<IMessageConsumerSource> _consumerSource;
        private readonly RabbitMqManager _manager;
        private readonly IMessageSerializer _messageSerializer;
        private readonly IPerformanceLog _performanceLog;
        private readonly ILog _log;


        public override void OnAfterStart()
        {
            try
            {
                _log.Info(Resources.InitializationOfConsumersStarted);

                var consumers = _consumerSource.SelectMany(source => source.GetConsumers()).ToList();
                var taskConsumers = consumers.OfType<ITaskConsumer>().ToList();
                var broadcastConsumers = consumers.OfType<IBroadcastConsumer>().ToList();

                InitializeTaskConsumers(taskConsumers);

                _log.Info(Resources.InitializationOfTaskConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "taskCounsumerCount", taskConsumers.Count } });

                InitializeBroadcastConsumers(broadcastConsumers);

                _log.Info(Resources.InitializationOfBroadcastConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "broadcastConsumerCount", broadcastConsumers.Count } });
            }
            catch (Exception e)
            {
                _log.Error(Resources.UnableToInitializeConsumers, e);
            }
        }

        private void InitializeTaskConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumer.GetType().Name);

                _log.Debug(Resources.InitializationOfTaskConsumerStarted, logContext);

                var channel = _manager.GetChannel();
                var queueName = QueueNamingConventions.GetConsumerQueueName(consumer);
                _manager.DeclareTaskQueue(queueName);

                InitializeConsumer(queueName, channel, consumer);

                _log.Debug(Resources.InitializationOfTaskConsumerSuccessfullyCompleted, logContext);
            }
        }

        private void InitializeBroadcastConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumer.GetType().Name);

                _log.Debug(Resources.InitializationOfBroadcastConsumerStarted, logContext);

                var channel = _manager.GetChannel();
                var routingKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                InitializeConsumer(queueName, channel, consumer);

                _log.Debug(Resources.InitializationOfBroadcastConsumerSuccessfullyCompleted, logContext);
            }
        }

        private void InitializeConsumer(string queueName, IModel channel, IConsumer consumer)
        {
            if (queueName == null)
            {
                throw new ArgumentException(Resources.UnableToGetQueueName);
            }

            var consumerType = consumer.GetType().Name;

            var eventingConsumer = new EventingBasicConsumer(channel);

            eventingConsumer.Received += (o, args) =>
                                         {
                                             var startDate = DateTime.Now;

                                             IMessage message;

                                             Func<Dictionary<string, object>> logContext = () => CreateLogContext(consumerType, args.DeliveryTag);

                                             try
                                             {
                                                 message = _messageSerializer.BytesToMessage(args, consumer.MessageType);
                                             }
                                             catch (Exception exception)
                                             {
                                                 _log.Error(exception, logContext);

                                                 return;
                                             }

                                             Task.Run(async () =>
                                                            {
                                                                startDate = DateTime.Now;

                                                                _log.Debug(Resources.ConsumeStart, logContext);

                                                                await consumer.Consume(message);

                                                                _log.Debug(Resources.ConsumeSuccess, logContext);
                                                            })
                                                 .ContinueWith(task =>
                                                               {
                                                                   try
                                                                   {
                                                                       if (task.IsFaulted)
                                                                       {
                                                                           _log.Error(task.Exception, logContext);
                                                                       }
                                                                       else
                                                                       {
                                                                           _log.Debug(Resources.AckStart, logContext);

                                                                           channel.BasicAck(args.DeliveryTag, false);

                                                                           _log.Debug(Resources.AckSuccess, logContext);
                                                                       }
                                                                   }
                                                                   finally
                                                                   {
                                                                       _performanceLog.Log(consumerType, startDate, task.Exception);
                                                                   }
                                                               });
                                         };

            channel.BasicConsume(queueName, false, eventingConsumer);
        }


        private static Dictionary<string, object> CreateLogContext(string consumerType, object deliveryTag = null)
        {
            return new Dictionary<string, object>
                   {
                       { "consumerType", consumerType },
                       { "deliveryTag", deliveryTag }
                   };
        }
    }
}