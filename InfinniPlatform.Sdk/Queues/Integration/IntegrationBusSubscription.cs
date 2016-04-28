namespace InfinniPlatform.Sdk.Queues.Integration
{
    /// <summary>
    ///     Информация о подписке на очередь сообщений интеграционной шины.
    /// </summary>
    public sealed class IntegrationBusSubscription
    {
        /// <summary>
        ///     Уникальный идентификатор обработчика очереди сообщений.
        /// </summary>
        public string ConsumerId { get; set; }

        /// <summary>
        ///     Тип точки обмена.
        /// </summary>
        public string ExchangeType { get; set; }

        /// <summary>
        ///     Наименование точки обмена.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        ///     Наименование очереди сообщений.
        /// </summary>
        public string QueueName { get; set; }

        /// <summary>
        ///     Ключ маршрутизации очереди сообщений.
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        ///     Заголовок маршрутизации очереди сообщений.
        /// </summary>
        public MessageHeaders RoutingHeaders { get; set; }

        /// <summary>
        ///     Адрес сервиса подписчика для отправки сообщений.
        /// </summary>
        public string Address { get; set; }
    }
}