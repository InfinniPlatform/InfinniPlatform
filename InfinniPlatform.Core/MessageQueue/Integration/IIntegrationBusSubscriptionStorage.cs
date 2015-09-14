namespace InfinniPlatform.MessageQueue.Integration
{
    /// <summary>
    ///     Хранилище подписок интеграционной шины.
    /// </summary>
    public interface IIntegrationBusSubscriptionStorage
    {
        /// <summary>
        ///     Получить aдрес сервиса подписчика для отправки сообщений из интеграционной шины.
        /// </summary>
        /// <param name="consumerId">Уникальный идентификатор обработчика очереди сообщений.</param>
        string GetSubscriptionAddress(string consumerId);

        /// <summary>
        ///     Сохранить информацию о подписке на очередь сообщений интеграционной шины.
        /// </summary>
        /// <param name="subscription">Информация о подписке на очередь сообщений интеграционной шины.</param>
        void AddSubscription(IntegrationBusSubscription subscription);

        /// <summary>
        ///     Удалить информацию о подписке на очередь сообщений интеграционной шины.
        /// </summary>
        /// <param name="consumerId">Уникальный идентификатор обработчика очереди сообщений.</param>
        void RemoveSubscription(string consumerId);
    }
}