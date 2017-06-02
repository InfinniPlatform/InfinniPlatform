using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Hosting;
using InfinniPlatform.MessageQueue.Management;
using InfinniPlatform.MessageQueue.Properties;

using Microsoft.Extensions.Logging;

using RabbitMQ.Client;

namespace InfinniPlatform.MessageQueue.Hosting
{
    public class RabbitMqMessageQueueInitializer : IAppStartedHandler, IAppStoppedHandler
    {
        /// <summary>
        /// Регистрирует потребителей сообщений.
        /// </summary>
        /// <param name="consumersManager">Предоставляет метод регистрации получателей сообщений из очереди.</param>
        /// <param name="consumerSource">Источники потребителей сообщений.</param>
        /// <param name="manager">Менеджер соединения с RabbitMQ.</param>
        /// <param name="logger">Лог.</param>
        public RabbitMqMessageQueueInitializer(IMessageQueueConsumersManager consumersManager,
                                               IEnumerable<IConsumerSource> consumerSource,
                                               RabbitMqManager manager,
                                               ILogger<RabbitMqMessageQueueInitializer> logger)
        {
            var consumers = consumerSource.SelectMany(source => source.GetConsumers()).ToList();
            _taskConsumers = consumers.OfType<ITaskConsumer>().ToList();
            _broadcastConsumers = consumers.OfType<IBroadcastConsumer>().ToList();

            manager.OnReconnect += (sender, args) => { HandleStart(); };

            _consumersManager = consumersManager;
            _manager = manager;
            _logger = logger;
        }

        private readonly List<IBroadcastConsumer> _broadcastConsumers;
        private readonly IMessageQueueConsumersManager _consumersManager;

        private readonly ILogger _logger;
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
                _logger.LogInformation(Resources.InitializationOfConsumersStarted);

                InitializeTaskConsumers();
                InitializeBroadcastConsumers();
            }
            catch (Exception e)
            {
                _logger.LogError(Resources.UnableToInitializeConsumers, e);
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
                _logger.LogDebug(Resources.InitializationOfTaskConsumerStarted, () => CreateLogContext(consumerType));

                var key = RabbitMqHelper.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareTaskQueue(key);

                _consumersManager.RegisterConsumer(queueName, consumer);

                _logger.LogDebug(Resources.InitializationOfTaskConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }

            _logger.LogInformation(Resources.InitializationOfTaskConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "taskCounsumerCount", _taskConsumers.Count } });
        }

        private void InitializeBroadcastConsumers()
        {
            foreach (var consumer in _broadcastConsumers)
            {
                var consumerType = consumer.GetType().Name;
                _logger.LogDebug(Resources.InitializationOfBroadcastConsumerStarted, () => CreateLogContext(consumerType));

                var routingKey = RabbitMqHelper.GetConsumerQueueName(consumer);
                var queueName = _manager.DeclareBroadcastQueue(routingKey);

                _consumersManager.RegisterConsumer(queueName, consumer);

                _logger.LogDebug(Resources.InitializationOfBroadcastConsumerSuccessfullyCompleted, () => CreateLogContext(consumerType));
            }

            _logger.LogInformation(Resources.InitializationOfBroadcastConsumersSuccessfullyCompleted, () => new Dictionary<string, object> { { "broadcastConsumerCount", _broadcastConsumers.Count } });
        }

        private static Dictionary<string, object> CreateLogContext(string consumerType)
        {
            return new Dictionary<string, object> { { "consumerType", consumerType } };
        }
    }
}