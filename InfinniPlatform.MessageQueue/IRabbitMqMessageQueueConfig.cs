using System;

using InfinniPlatform.MessageQueue.RabbitMq.Policies;
using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue
{
	/// <summary>
	/// Конфигурация для создания инфраструктурных сервисов очереди сообщений на базе RabbitMq.
	/// </summary>
	public interface IRabbitMqMessageQueueConfig
	{
		/// <summary>
		/// Политика для определения возможности повторного выполнения действия после неудачной попытки.
		/// </summary>
		IRetryPolicy RetryPolicy { get; }

		/// <summary>
		/// Политика для определения задержки между попытками повторного выполнения действия.
		/// </summary>
		IRetrySchedulePolicy RetrySchedulePolicy { get; }

		/// <summary>
		/// Метод для настройки по умолчанию свойств точки обмена сообщениями.
		/// </summary>
		Action<IExchangeConfig> DefaultExchangeConfig { get; }

		/// <summary>
		/// Метод для настройки по умолчанию свойств очереди сообщений.
		/// </summary>
		Action<IQueueConfig> DefaultQueueConfig { get; }
	}
}