namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Сервис для публикации сообщений.
    /// </summary>
    public interface IMessageQueuePublisher
    {
        /// <summary>
        ///     Опубликовать сообщение.
        /// </summary>
        /// <param name="exchange">Наименование точки обмена сообщениями.</param>
        /// <param name="routingKey">Ключ маршрутизации сообщения.</param>
        /// <param name="properties">Свойства сообщения.</param>
        /// <param name="body">Тело сообщения.</param>
        void Publish(string exchange, string routingKey, MessageProperties properties, byte[] body);
    }
}