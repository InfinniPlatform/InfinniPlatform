using InfinniPlatform.Api.ClientNotification;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;

namespace InfinniPlatform.ContextComponents
{
    /// <summary>
    /// Компонент для осуществления клиентской нотификации из контекста
    /// </summary>
    public sealed class WebClientNotificationComponent : IWebClientNotificationComponent
    {
        public WebClientNotificationComponent(IWebClientNotificationServiceFactory factory)
        {
            _notificationService = factory.CreateClientNotificationService();
        }

        private readonly IWebClientNotificationService _notificationService;

        public void Notify(string routingKey, object message)
        {
            _notificationService.Notify(routingKey, message);
        }
    }
}