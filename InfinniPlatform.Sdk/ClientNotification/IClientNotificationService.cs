namespace InfinniPlatform.Sdk.ClientNotification
{
    /// <summary>
    /// Сервис оповещения Web-клиентов.
    /// </summary>
    public interface IClientNotificationService
    {
        /// <summary>
        /// Оповестить клиентов.
        /// </summary>
        /// <param name="routingKey">Ключ маршрутизации.</param>
        /// <param name="message">Сообщение.</param>
        void Notify(string routingKey, object message);
    }
}