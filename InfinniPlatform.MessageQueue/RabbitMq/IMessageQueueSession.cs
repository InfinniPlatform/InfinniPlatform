using System;

using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Outdated;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
	/// <summary>
	/// Сессия очереди сообщений.
	/// </summary>
	public interface IMessageQueueSession : IDisposable
	{
		/// <summary>
		/// Является ли сессия открытой и готовой к использованию.
		/// </summary>
		bool IsOpen { get; }


		/// <summary>
		/// Существует ли точка обмена.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена.</param>
		bool ExchangeExists(string exchangeName);

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Fanout".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		void CreateExchangeFanout(ExchangeConfig exchangeConfig);

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Direct".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		void CreateExchangeDirect(ExchangeConfig exchangeConfig);

		/// <summary>
		/// Создать точку обмена сообщениями с типом "Direct".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		void CreateExchangeTopic(ExchangeConfig exchangeConfig);

		/// <summary>
		/// Получить точку обмена сообщениями с типом "Headers".
		/// </summary>
		/// <param name="exchangeConfig">Свойства точки обмена сообщениями.</param>
		void CreateExchangeHeaders(ExchangeConfig exchangeConfig);

		/// <summary>
		/// Удалить точку обмена.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена.</param>
		void DeleteExchange(string exchangeName);


		/// <summary>
		/// Существует ли очередь сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		bool QueueExists(string queueName);

		/// <summary>
		/// Создать очередь сообщений типа "Fanout".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		IMessageQueue CreateQueueFanout(QueueConfig queueConfig);

		/// <summary>
		/// Создать очередь сообщений типа "Direct".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingKey">Ключ маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		IMessageQueue CreateQueueDirect(QueueConfig queueConfig, string routingKey);

		/// <summary>
		/// Создать очередь сообщений типа "Topic".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingPattern">Шаблон маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		IMessageQueue CreateQueueTopic(QueueConfig queueConfig, string routingPattern);

		/// <summary>
		/// Создать очередь сообщений типа "Headers".
		/// </summary>
		/// <param name="queueConfig">Свойства очереди сообщений.</param>
		/// <param name="routingHeader">Заголовок маршрутизации очереди сообщений.</param>
		/// <returns>Очередь сообщений.</returns>
		IMessageQueue CreateQueueHeaders(QueueConfig queueConfig, MessageHeaders routingHeader);

		/// <summary>
		/// Удалить очередь сообщений.
		/// </summary>
		/// <param name="queueName">Наименование очереди сообщений.</param>
		void DeleteQueue(string queueName);


		/// <summary>
		/// Опубликовать сообщение.
		/// </summary>
		/// <param name="exchangeName">Наименование точки обмена сообщениями.</param>
		/// <param name="routingKey">Ключ маршрутизации сообщения.</param>
		/// <param name="properties">Свойства сообщения.</param>
		/// <param name="body">Тело сообщения.</param>
		void Publish(string exchangeName, string routingKey, MessageProperties properties, byte[] body);
	}
}