using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.MessageQueue.Abstractions;
using InfinniPlatform.MessageQueue.Abstractions.Consumers;
using InfinniPlatform.MessageQueue.RabbitMq.Management;
using InfinniPlatform.MessageQueue.RabbitMQ.Properties;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.Logging;
using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.RabbitMQ.Hosting
{
    internal sealed class MessageQueueInitializer : IAppStartedHandler, IAppStoppedHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="consumersManager">Предоставляет метод регистрации получателей сообщений из очереди.</param>
        /// <param name="consumerSource">Источники потребителей сообщений.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="log">Лог.</param>
        public MessageQueueInitializer(IMessageQueueConsumersManager consumersManager,
                                       IEnumerable<IMessageConsumerSource> consumerSource,
                                       RabbitMqManager manager,
                                       ILog log)
        {
            var consumers = consumerSource.SelectMany(source => source.GetConsumers()).ToList();
            _taskConsumers = consumers.OfType<ITaskConsumer>().ToList();
            _broadcastConsumers = consumers.OfType<IBroadcastConsumer>().ToList();

            manager.OnReconnect += (sender, args) => { HandleStart(); };

            _consumersManager = consumersManager;
            _manager = manager;
            _log = log;
        }

        private readonly List<IBroadcastConsumer> _broadcastConsumers;
        private readonly IMessageQueueConsumersManager _consumersManager;

        private readonly ILog _log;
        private readonly RabbitMqManager _manager;
        private readonly List<ITaskConsumer> _taskConsumers;

        void IAppStartedHandler.Handle()
        {
            HandleStart();
        }

        private void HandleStart()
        {
            try
            {
                _log.Info(Resources.InitializationOfConsumersStarted);

                InitializeTaskConsumers();
                InitializeBroadcastConsumers();
            }
            catch (Exception e)
            {
                _log.Error(Resources.UnableToInitializeConsumers, e);
            }
        }

        void IAppStoppedHandler.Handle()
        {
            _manager.Dispose();
        }

        private void RegisterOnReconnectEvent()
        {
            var connection = _manager.Connection;
            var recoverable = connection as IRecoverable;
            if (recoverable != null)
            {
                recoverable.Recovery += (sender, args) =>
                                        {
                                            //InitializeBroadcastConsumers();
                                        };
            }
        }

        private void InitializeTaskConsumers()
        {
            foreach (var consumer in _taskConsumers)
            {
                var consumerType = consumer.GetType().Name;
                _log.Debug(Resources.InitializationOfTaskConsumerStarted, () => CreateLogContext(consumerType));

                var key = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareTaskQueue(key);

                _consumersManager.RegisterConsumer(queueName, consumer);

                _log.Debug(Resources.InitializationOfTaskConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }

            _log.Info(Resources.InitializationOfTaskConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "taskCounsumerCount", _taskConsumers.Count } });
        }

        private void InitializeBroadcastConsumers()
        {
            foreach (var consumer in _broadcastConsumers)
            {
                var consumerType = consumer.GetType().Name;
                _log.Debug(Resources.InitializationOfBroadcastConsumerStarted, () => CreateLogContext(consumerType));

                var routingKey = QueueNamingConventions.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                _consumersManager.RegisterConsumer(queueName, consumer);

                _log.Debug(Resources.InitializationOfBroadcastConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }

            _log.Info(Resources.InitializationOfBroadcastConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "broadcastConsumerCount", _broadcastConsumers.Count } });
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