using System;

namespace InfinniPlatform.MessageQueue
{
	/// <summary>
	/// Интерфейс для объявления очередей, связанных с точкой обмена сообщениями типа "Direct".
	/// </summary>
	/// <remarks>
	/// Маршрутизация сообщений для точек обмена с типом "Direct" осуществляется от издателя во все очереди, связанные с точкой обмена и
	/// имеющие ключ маршрутизации, который совпадает с ключом маршрутизации, указанным при отправке сообщения.
	/// </remarks>
	public interface IExchangeDirectBinding
	{
		/// <summary>
		/// Подписаться на очередь сообщений.
		/// </summary>
		/// <param name="queue">Наименование очереди сообщений.</param>
		/// <param name="consumer">Метод для получения прослушивателя очереди сообщений.</param>
		/// <param name="routingKey">Ключ маршрутизации очереди сообщений.</param>
		/// <param name="config">Метод для настройки свойств очереди сообщений.</param>
		/// <remarks>
		/// При формировании ключа маршрутизации используются следующие соглашения. Ключ должен быть пустым, либо состоять из слов,
		/// разделенных точками. Каждое слово может содержать симоволы из набора [a-zA-Z0-9].
		/// </remarks>
		void Subscribe(string queue, Func<IQueueConsumer> consumer, string routingKey, Action<IQueueConfig> config = null);

		/// <summary>
		/// Удалить подписку на очередь сообщений.
		/// </summary>
		/// <param name="queue">Наименование очереди сообщений.</param>
		/// <remarks>
		/// При удалении подписки на очередь сообщений, удаляется очередь и все сообщения, находящиеся в ней.
		/// </remarks>
		void Unsubscribe(string queue);
	}
}