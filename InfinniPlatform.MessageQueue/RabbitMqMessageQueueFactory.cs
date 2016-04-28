using System;

using InfinniPlatform.Core.Factories;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.MessageQueue.RabbitMq;
using InfinniPlatform.MessageQueue.RabbitMq.Client;
using InfinniPlatform.MessageQueue.RabbitMq.Policies;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    /// Фабрика для создания инфраструктурных сервисов очереди сообщений на базе RabbitMq.
    /// </summary>
    public sealed class RabbitMqMessageQueueFactory : IMessageQueueFactory
    {
        public RabbitMqMessageQueueFactory(RabbitMqSettings settings, ILog log) : this(settings, log, new RabbitMqMessageQueueDefaultConfig(log))
        {
        }

        public RabbitMqMessageQueueFactory(RabbitMqSettings settings, ILog log, IRabbitMqMessageQueueConfig config)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (config == null)
            {
                throw new ArgumentNullException(nameof(config));
            }

            _defaultExchangeConfig = config.DefaultExchangeConfig;
            _defaultQueueConfig = config.DefaultQueueConfig;
            _commandExecutor = new RetryCommandExecutor(config.RetryPolicy, config.RetrySchedulePolicy);
            _sessionFactory = new RabbitMqSessionFactory(settings.Node, settings.Port);
            _queueListener = new MessageQueueListener();
        }


        private readonly Action<IExchangeConfig> _defaultExchangeConfig;
        private readonly Action<IQueueConfig> _defaultQueueConfig;
        private readonly RetryCommandExecutor _commandExecutor;
        private readonly RabbitMqSessionFactory _sessionFactory;
        private readonly MessageQueueListener _queueListener;


        /// <summary>
        /// Создать сервис для управления рабочими потоками очередей сообщений.
        /// </summary>
        public IMessageQueueListener CreateMessageQueueListener()
        {
            return _queueListener;
        }


        private IMessageQueueManager _queueManager;

        /// <summary>
        /// Создать сервис для управления подписками на очереди сообщений.
        /// </summary>
        public IMessageQueueManager CreateMessageQueueManager()
        {
            if (_queueManager == null)
            {
                // Сессия на команду, чтобы обеспечить изоляцию при выполнении команд
                var sessionManager = new MessageQueueSessionManagerPerCommand(_sessionFactory);
                var commandExecutor = new MessageQueueCommandExecutor(_commandExecutor, sessionManager);

                _queueManager = new MessageQueueManager(commandExecutor, _queueListener, _defaultExchangeConfig, _defaultQueueConfig);
            }

            return _queueManager;
        }


        private IMessageQueuePublisher _queuePublisher;

        /// <summary>
        /// Создать сервис для публикации сообщений.
        /// </summary>
        public IMessageQueuePublisher CreateMessageQueuePublisher()
        {
            if (_queuePublisher == null)
            {
                // Сессия на поток, чтобы иметь возможность быстрой публикации сообщений
                var sessionManager = new MessageQueueSessionManagerPerThread(_sessionFactory);
                var commandExecutor = new MessageQueueCommandExecutor(_commandExecutor, sessionManager);

                _queuePublisher = new MessageQueuePublisher(commandExecutor);
            }

            return _queuePublisher;
        }


        private class RabbitMqMessageQueueDefaultConfig : IRabbitMqMessageQueueConfig
        {
            private static readonly IRetryPolicy DefaultRetryPolicy = new ConstantRetryPolicy(new RabbitMqRetryDecisionProvider(), 5);

            private static readonly IRetrySchedulePolicy DefaultRetrySchedulePolicy = new ExponentialRetrySchedulePolicy(2000, 300000);

            private static readonly IAcknowledgePolicy DefaultAcknowledgePolicy = new AlwaysAfterHandlingAcknowledgePolicy();

            private static readonly IRejectPolicy DefaultRejectPolicy = new NeverRejectPolicy();


            public RabbitMqMessageQueueDefaultConfig(ILog log)
            {
                _log = log;
            }

            private readonly ILog _log;

            private void DefaultWorkerThreadErrorHandler(Exception error)
            {
                _log.Error(error.Message, null, error);
            }

            private void DefaultConsumerErrorHandler(Exception error)
            {
                _log.Error(error.Message, null, error);
            }

            private static void DefaultExchangeConfigAction(IExchangeConfig config)
            {
            }

            private void DefaultQueueConfigAction(IQueueConfig config)
            {
                config.PrefetchCount(10)
                      .WorkerThreadCount(2)
                      .MinListenTime(120000)
                      .AcknowledgePolicy(DefaultAcknowledgePolicy)
                      .RejectPolicy(DefaultRejectPolicy)
                      .WorkerThreadError(DefaultWorkerThreadErrorHandler)
                      .ConsumerError(DefaultConsumerErrorHandler);
            }


            public IRetryPolicy RetryPolicy
            {
                get { return DefaultRetryPolicy; }
            }

            public IRetrySchedulePolicy RetrySchedulePolicy
            {
                get { return DefaultRetrySchedulePolicy; }
            }

            public Action<IExchangeConfig> DefaultExchangeConfig
            {
                get { return DefaultExchangeConfigAction; }
            }

            public Action<IQueueConfig> DefaultQueueConfig
            {
                get { return DefaultQueueConfigAction; }
            }
        }
    }
}