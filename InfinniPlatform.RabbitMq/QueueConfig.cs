using System;

using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.RabbitMq
{
	/// <summary>
	/// Свойства очереди сообщений.
	/// </summary>
	public sealed class QueueConfig : IQueueConfig
	{
		public QueueConfig(string exchangeName, string queueName)
		{
			if (string.IsNullOrWhiteSpace(exchangeName))
			{
				throw new ArgumentNullException("exchangeName");
			}

			if (string.IsNullOrWhiteSpace(queueName))
			{
				throw new ArgumentNullException("queueName");
			}

			ExchangeName = exchangeName;
			QueueName = queueName;

			QueueWorkerThreadCount = 1;
		}


		/// <summary>
		/// Наименование точки обмена.
		/// </summary>
		public string ExchangeName { get; private set; }

		/// <summary>
		/// Наименование очереди сообщений.
		/// </summary>
		public string QueueName { get; private set; }

		/// <summary>
		/// Сохранять сообщения на диске.
		/// </summary>
		public bool QueueDurable { get; set; }

		/// <summary>
		/// Уникальный идентификатор обработчика очереди сообщений.
		/// </summary>
		public string QueueConsumerId { get; set; }

		/// <summary>
		/// Метод обработки ошибок обработчика очереди сообщений.
		/// </summary>
		public Action<Exception> QueueConsumerError { get; set; }

		/// <summary>
		/// Максимальный размер одновременно обрабатываемых сообщений.
		/// </summary>
		public int QueuePrefetchSize { get; set; }

		/// <summary>
		/// Максимальное количество одновременно обрабатываемых сообщений.
		/// </summary>
		public int QueuePrefetchCount { get; set; }

		/// <summary>
		/// Количество рабочих потоков для обработки очереди сообщений.
		/// </summary>
		public int QueueWorkerThreadCount { get; set; }

		/// <summary>
		/// Метод обработки ошибок рабочего потока очереди сообщений.
		/// </summary>
		public Action<Exception> QueueWorkerThreadError { get; set; }

		/// <summary>
		/// Минимальное время прослушивания очереди, которое не считается сетевым сбоем.
		/// </summary>
		public int QueueMinListenTime { get; set; }

		/// <summary>
		/// Политика подтверждения окончания обработки сообщения.
		/// </summary>
		public IAcknowledgePolicy QueueAcknowledgePolicy { get; set; }

		/// <summary>
		/// Политика подтверждения отказа от обработки сообения.
		/// </summary>
		public IRejectPolicy QueueRejectPolicy { get; set; }


		IQueueConfig IQueueConfig.Durable()
		{
			QueueDurable = true;

			return this;
		}

		IQueueConfig IQueueConfig.ConsumerId(string value)
		{
			QueueConsumerId = value;

			return this;
		}

		IQueueConfig IQueueConfig.ConsumerError(Action<Exception> value)
		{
			QueueConsumerError = value;

			return this;
		}

		IQueueConfig IQueueConfig.PrefetchSize(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			QueuePrefetchSize = value;

			return this;
		}

		IQueueConfig IQueueConfig.PrefetchCount(int value)
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			QueuePrefetchCount = value;

			return this;
		}

		IQueueConfig IQueueConfig.WorkerThreadCount(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			QueueWorkerThreadCount = value;

			return this;
		}

		IQueueConfig IQueueConfig.WorkerThreadError(Action<Exception> value)
		{
			QueueWorkerThreadError = value;

			return this;
		}

		IQueueConfig IQueueConfig.MinListenTime(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException();
			}

			QueueMinListenTime = value;

			return this;
		}

		IQueueConfig IQueueConfig.AcknowledgePolicy(IAcknowledgePolicy value)
		{
			QueueAcknowledgePolicy = value;

			return this;
		}

		IQueueConfig IQueueConfig.RejectPolicy(IRejectPolicy value)
		{
			QueueRejectPolicy = value;

			return this;
		}
	}
}