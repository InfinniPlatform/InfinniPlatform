using System;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Интерфейс для объявления очередей, связанных с точкой обмена сообщениями типа "Headers".
    /// </summary>
    /// <remarks>
    ///     Маршрутизация сообщений для точек обмена с типом "Headers" осуществляется от издателя во все очереди, связанные с
    ///     точкой обмена и
    ///     имеющие заголовок маршрутизации, который совпадает с заголовком маршрутизации, указанным при отправке сообщения.
    /// </remarks>
    public interface IExchangeHeadersBinding
    {
        /// <summary>
        ///     Подписаться на очередь сообщений.
        /// </summary>
        /// <param name="queue">Наименование очереди сообщений.</param>
        /// <param name="consumer">Метод для получения прослушивателя очереди сообщений.</param>
        /// <param name="routingHeaders">Заголовок маршрутизации очереди сообщений.</param>
        /// <param name="config">Метод для настройки свойств очереди сообщений.</param>
        void Subscribe(string queue, Func<IQueueConsumer> consumer, MessageHeaders routingHeaders,
            Action<IQueueConfig> config = null);

        /// <summary>
        ///     Удалить подписку на очередь сообщений.
        /// </summary>
        /// <param name="queue">Наименование очереди сообщений.</param>
        /// <remarks>
        ///     При удалении подписки на очередь сообщений, удаляется очередь и все сообщения, находящиеся в ней.
        /// </remarks>
        void Unsubscribe(string queue);
    }
}