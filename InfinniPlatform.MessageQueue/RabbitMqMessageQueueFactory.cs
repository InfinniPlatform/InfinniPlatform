using System;

using InfinniPlatform.Factories;
using InfinniPlatform.Logging;
using InfinniPlatform.RabbitMq;
using InfinniPlatform.RabbitMq.Client;
using InfinniPlatform.RabbitMq.Policies;

namespace InfinniPlatform.MessageQueue
{
	/// <summary>
	/// Фабрика для создания инфраструктурных сервисов очереди сообщений на базе RabbitMq.
	/// </summary>
	public sealed class RabbitMqMessageQueueFactory : IMessageQueueFactory
	{
		public RabbitMqMessageQueueFactory()
			: this(new RabbitMqMessageQueueDefaultConfig())
		{
		}

		public RabbitMqMessageQueueFactory(IRabbitMqMessageQueueConfig config)
		{
			if (config == null)
			{
				throw new ArgumentNullException("config");
			}

			_defaultExchangeConfig = config.DefaultExchangeConfig;
			_defaultQueueConfig = config.DefaultQueueConfig;
			_commandExecutor = new RetryCommandExecutor(config.RetryPolicy, config.RetrySchedulePolicy);
			_sessionFactory = new RabbitMqSessionFactory();
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


		class RabbitMqMessageQueueDefaultConfig : IRabbitMqMessageQueueConfig
		{
			private static readonly IRetryPolicy DefaultRetryPolicy = new ConstantRetryPolicy(new RabbitMqRetryDecisionProvider(), 5);

			private static readonly IRetrySchedulePolicy DefaultRetrySchedulePolicy = new ExponentialRetrySchedulePolicy(2000, 300000);

			private static readonly IAcknowledgePolicy DefaultAcknowledgePolicy = new AlwaysAfterHandlingAcknowledgePolicy();

			private static readonly IRejectPolicy DefaultRejectPolicy = new NeverRejectPolicy();

			private static void DefaultWorkerThreadErrorHandler(Exception error)
			{
				Logger.Log.Fatal(error.Message, error);
			}

			private static void DefaultConsumerErrorHandler(Exception error)
			{
				Logger.Log.Error(error.Message, error);
			}

			private static void DefaultExchangeConfigAction(IExchangeConfig config)
			{
			}

			private static void DefaultQueueConfigAction(IQueueConfig config)
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