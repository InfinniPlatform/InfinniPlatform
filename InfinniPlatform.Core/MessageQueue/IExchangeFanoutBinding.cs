using System;

namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Интерфейс для объявления очередей, связанных с точкой обмена сообщениями типа "Fanout".
    /// </summary>
    /// <remarks>
    ///     Маршрутизация сообщений для точек обмена с типом "Fanout" осуществляется от издателя во все очереди, связанные с
    ///     точкой обмена.
    /// </remarks>
    public interface IExchangeFanoutBinding
    {
        /// <summary>
        ///     Подписаться на очередь сообщений.
        /// </summary>
        /// <param name="queue">Наименование очереди сообщений.</param>
        /// <param name="consumer">Метод для получения прослушивателя очереди сообщений.</param>
        /// <param name="config">Метод для настройки свойств очереди сообщений.</param>
        void Subscribe(string queue, Func<IQueueConsumer> consumer, Action<IQueueConfig> config = null);

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