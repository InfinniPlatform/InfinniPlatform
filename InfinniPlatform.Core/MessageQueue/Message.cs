namespace InfinniPlatform.MessageQueue
{
    /// <summary>
    ///     Сообщение очереди.
    /// </summary>
    public sealed class Message
    {
        /// <summary>
        ///     Уникальный идентификатор сообещения в очереди сообщений.
        /// </summary>
        public ulong DeliveryTag { get; set; }

        /// <summary>
        ///     Уникальный идентификатор обработчика очереди сообщений.
        /// </summary>
        public string ConsumerId { get; set; }

        /// <summary>
        ///     Наименование точки обмена сообщениями.
        /// </summary>
        public string Exchange { get; set; }

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