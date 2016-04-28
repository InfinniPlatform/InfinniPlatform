using System;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Реализует интерфейс для объявления очередей, связанных с точкой обмена сообщениями.
	/// </summary>
	sealed class ExchangeBinding : IExchangeFanoutBinding, IExchangeDirectBinding, IExchangeTopicBinding, IExchangeHeadersBinding
	{
		public ExchangeBinding(string exchangeName, IMessageQueueCommandExecutor queueCommandExecutor, IMessageQueueWorkerContainer queueWorkerContainer, Action<IQueueConfig> defaultQueueConfig = null)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException("exchangeName");
			}

			if (queueCommandExecutor == null)
			{
				throw new ArgumentNullException("queueCommandExecutor");
			}

			if (queueWorkerContainer == null)
			{
				throw new ArgumentNullException("queueWorkerContainer");
			}

			_exchangeName = exchangeName;
			_queueCommandExecutor = queueCommandExecutor;
			_queueWorkerContainer = queueWorkerContainer;
			_defaultQueueConfig = defaultQueueConfig;
		}


		private readonly string _exchangeName;
		private readonly IMessageQueueCommandExecutor _queueCommandExecutor;
		private readonly IMessageQueueWorkerContainer _queueWorkerContainer;
		private readonly Action<IQueueConfig> _defaultQueueConfig;


		// IExchangeFanoutBinding

		void IExchangeFanoutBinding.Subscribe(string queue, Func<IQueueConsumer> consumer, Action<IQueueConfig> config)
		{
			var queueConfig = CreateQueueConfig(queue, config);

			CreateQueue(queueConfig, consumer, session => session.CreateQueueFanout(queueConfig));
		}

		void IExchangeFanoutBinding.Unsubscribe(string queue)
		{
			UnsubscribeQueue(queue);
		}


		// IExchangeDirectBinding

		void IExchangeDirectBinding.Subscribe(string queue, Func<IQueueConsumer> consumer, string routingKey, Action<IQueueConfig> config)
		{
			var queueConfig = CreateQueueConfig(queue, config);

			CreateQueue(queueConfig, consumer, session => session.CreateQueueDirect(queueConfig, routingKey));
		}

		void IExchangeDirectBinding.Unsubscribe(string queue)
		{
			UnsubscribeQueue(queue);
		}


		// IExchangeTopicBinding

		void IExchangeTopicBinding.Subscribe(string queue, Func<IQueueConsumer> consumer, string routingPattern, Action<IQueueConfig> config)
		{
			var queueConfig = CreateQueueConfig(queue, config);

			CreateQueue(queueConfig, consumer, session => session.CreateQueueTopic(queueConfig, routingPattern));
		}

		void IExchangeTopicBinding.Unsubscribe(string queue)
		{
			UnsubscribeQueue(queue);
		}


		// IExchangeHeadersBinding

		void IExchangeHeadersBinding.Subscribe(string queue, Func<IQueueConsumer> consumer, MessageHeaders routingHeaders, Action<IQueueConfig> config)
		{
			var queueConfig = CreateQueueConfig(queue, config);

			CreateQueue(queueConfig, consumer, session => session.CreateQueueHeaders(queueConfig, routingHeaders));
		}

		void IExchangeHeadersBinding.Unsubscribe(string queue)
		{
			UnsubscribeQueue(queue);
		}


		private QueueConfig CreateQueueConfig(string queue, Action<IQueueConfig> queueConfig)
		{
			if (string.IsNullOrWhiteSpace(queue))
			{
				throw new ArgumentNullException("queue");
			}

			var config = new QueueConfig(_exchangeName, queue);

			if (_defaultQueueConfig != null)
			{
				_defaultQueueConfig(config);
			}

			if (queueConfig != null)
			{
				queueConfig(config);
			}

			return config;
		}

		private void CreateQueue(QueueConfig queueConfig, Func<IQueueConsumer> consumer, Func<IMessageQueueSession, IMessageQueue> queueFactory)
		{
			if (consumer == null)
			{
				throw new ArgumentNullException("consumer");
			}

			// Очередь должна быть создана вне зависимости от того, когда начнется ее прослушивание
			_queueCommandExecutor.Execute(session => queueFactory(session));


			// Создание менеджера для управления рабочим потоком очереди сообщений

			var queueWorker = new MessageQueueWorker(queueConfig, consumer, _queueCommandExecutor, queueFactory);

			_queueWorkerContainer.RegisterWorker(queueConfig.QueueName, queueWorker);
		}

		private void UnsubscribeQueue(string queue)
		{
			if (string.IsNullOrWhiteSpace(queue))
			{
				throw new ArgumentNullException("queue");
			}

			_queueWorkerContainer.UnregisterWorker(queue);

			_queueCommandExecutor.Execute(session => session.DeleteQueue(queue));
		}
	}
}