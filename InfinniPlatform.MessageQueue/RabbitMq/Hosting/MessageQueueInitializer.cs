using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.MessageQueue.Properties;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;

namespace InfinniPlatform.MessageQueue.RabbitMq.Hosting
{
    internal sealed class MessageQueueInitializer : AppEventHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="subscriptionManager">Предоставляет метод регистрации получателей сообщений из очереди.</param>
        /// <param name="consumerSource">Источники потребителей сообщений.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="log">Лог.</param>
        public MessageQueueInitializer(IMessageQueueSubscriptionManager subscriptionManager,
                                       IEnumerable<IMessageConsumerSource> consumerSource,
                                       RabbitMqManager manager,
                                       ILog log)
        {
            _subscriptionManager = subscriptionManager;
            _consumerSource = consumerSource;
            _manager = manager;
            _log = log;
        }

        private readonly IEnumerable<IMessageConsumerSource> _consumerSource;
        private readonly ILog _log;
        private readonly RabbitMqManager _manager;

        private readonly IMessageQueueSubscriptionManager _subscriptionManager;

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

        public override void OnAfterStop()
        {
            _manager.Dispose();
        }

        private void InitializeTaskConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var consumerType = consumer.GetType().Name;
                _log.Debug(Resources.InitializationOfTaskConsumerStarted, () => CreateLogContext(consumerType));

                var queueName = QueueNamingConventions.GetConsumerQueueName(consumer);
                _manager.DeclareTaskQueue(queueName);

                _subscriptionManager.RegisterConsumer(queueName, consumer);

                _log.Debug(Resources.InitializationOfTaskConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }
        }

        private void InitializeBroadcastConsumers(IEnumerable<IConsumer> consumers)
        {
            foreach (var consumer in consumers)
            {
                var consumerType = consumer.GetType().Name;
                _log.Debug(Resources.InitializationOfBroadcastConsumerStarted, () => CreateLogContext(consumerType));

                var routingKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                _subscriptionManager.RegisterConsumer(queueName, consumer);

                _log.Debug(Resources.InitializationOfBroadcastConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }
        }

        private static Dictionary<string, object> CreateLogContext(string consumerType)
        {
            return new Dictionary<string, object>
                   {
                       { "consumerType", consumerType }
                   };
        }
    }
}