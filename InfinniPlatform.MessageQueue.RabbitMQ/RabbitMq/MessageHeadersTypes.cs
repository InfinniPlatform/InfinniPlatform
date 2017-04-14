namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// Типы заголовков сообщения.
    /// </summary>
    public static class MessageHeadersTypes
    {
        /// <summary>
        /// Тип утверждения для хранения идентификатора текущей организации.
        /// </summary>
        public const string TenantId = "tenantid";

        /// <summary>
        /// Тип утверждения для хранения идентификатора организации по умолчанию.
        /// </summary>
        public const string UserName = "username";
    }
}