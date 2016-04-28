using System;

using InfinniPlatform.Core.MessageQueue;
using InfinniPlatform.MessageQueue.Properties;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Сервис для управления подписками на очереди сообщений.
	/// </summary>
	public sealed class MessageQueueManager : IMessageQueueManager
	{
		public MessageQueueManager(IMessageQueueCommandExecutor queueCommandExecutor, IMessageQueueWorkerContainer queueWorkerContainer, Action<IExchangeConfig> defaultExchangeConfig = null, Action<IQueueConfig> defaultQueueConfig = null)
		{
			if (queueCommandExecutor == null)
			{
				throw new ArgumentNullException("queueCommandExecutor");
			}

			if (queueWorkerContainer == null)
			{
				throw new ArgumentNullException("queueWorkerContainer");
			}

			_queueCommandExecutor = queueCommandExecutor;
			_queueWorkerContainer = queueWorkerContainer;
			_defaultExchangeConfig = defaultExchangeConfig;
			_defaultQueueConfig = defaultQueueConfig;
		}


		private readonly IMessageQueueCommandExecutor _queueCommandExecutor;
		private readonly IMessageQueueWorkerContainer _queueWorkerContainer;
		private readonly Action<IExchangeConfig> _defaultExchangeConfig;
		private readonly Action<IQueueConfig> _defaultQueueConfig;


		public IExchangeFanoutBinding GetExchangeFanout(string name)
		{
			return GetExchange(name);
		}

		public IExchangeFanoutBinding CreateExchangeFanout(string name, Action<IExchangeConfig> config = null)
		{
			return CreateExchange(name, config, (session, exchangeConfig) => session.CreateExchangeFanout(exchangeConfig));
		}


		public IExchangeDirectBinding GetExchangeDirect(string name)
		{
			return GetExchange(name);
		}

		public IExchangeDirectBinding CreateExchangeDirect(string name, Action<IExchangeConfig> config = null)
		{
			return CreateExchange(name, config, (session, exchangeConfig) => session.CreateExchangeDirect(exchangeConfig));
		}


		public IExchangeTopicBinding GetExchangeTopic(string name)
		{
			return GetExchange(name);
		}

		public IExchangeTopicBinding CreateExchangeTopic(string name, Action<IExchangeConfig> config = null)
		{
			return CreateExchange(name, config, (session, exchangeConfig) => session.CreateExchangeTopic(exchangeConfig));
		}

		public IExchangeHeadersBinding GetExchangeHeaders(string name)
		{
			return GetExchange(name);
		}

		public IExchangeHeadersBinding CreateExchangeHeaders(string name, Action<IExchangeConfig> config = null)
		{
			return CreateExchange(name, config, (session, exchangeConfig) => session.CreateExchangeHeaders(exchangeConfig));
		}


		private ExchangeBinding GetExchange(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			var isExists = false;

			_queueCommandExecutor.Execute(session => isExists = session.ExchangeExists(name));

			if (isExists == false)
			{
				throw new InvalidOperationException(string.Format(Resources.MessageQueueExchangeIsNotExists, name));
			}

			return new ExchangeBinding(name, _queueCommandExecutor, _queueWorkerContainer, _defaultQueueConfig);
		}

		private ExchangeBinding CreateExchange(string name, Action<IExchangeConfig> exchangeConfig, Action<IMessageQueueSession, ExchangeConfig> createExchange)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			var config = new ExchangeConfig(name);

			if (_defaultExchangeConfig != null)
			{
				_defaultExchangeConfig(config);
			}

			if (exchangeConfig != null)
			{
				exchangeConfig(config);
			}

			_queueCommandExecutor.Execute(session => createExchange(session, config));

			return new ExchangeBinding(name, _queueCommandExecutor, _queueWorkerContainer, _defaultQueueConfig);
		}
	}
}