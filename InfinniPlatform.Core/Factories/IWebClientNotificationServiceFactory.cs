using InfinniPlatform.Core.ClientNotification;

namespace InfinniPlatform.Core.Factories
{
    /// <summary>
    ///     Фабрика для создания сервиса оповещения Web-клиентов.
    /// </summary>
    public interface IWebClientNotificationServiceFactory
    {
        /// <summary>
        ///     Создать сервис оповещения Web-клиентов.
        /// </summary>
        IWebClientNotificationService CreateClientNotificationService();
    }
}