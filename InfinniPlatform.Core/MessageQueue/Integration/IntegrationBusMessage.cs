namespace InfinniPlatform.MessageQueue.Integration
{
    /// <summary>
    ///     Сообщение интеграционной шины.
    /// </summary>
    public sealed class IntegrationBusMessage
    {
        /// <summary>
        ///     Уникальный идентификатор обработчика очереди сообщений.
        /// </summary>
        public string ConsumerId { get; set; }

        /// <summary>
        ///     Наименование точки обмена.
        /// </summary>
        public string ExchangeName { get; set; }

        /// <summary>
        ///     Ключ маршрутизации сообщения.
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        ///     Свойства сообщения.
        /// </summary>
        public MessageProperties Properties { get; set; }

        /// <summary>
        ///     Тело сообщения.
        /// </summary>
        public byte[] Body { get; set; }
    }
}