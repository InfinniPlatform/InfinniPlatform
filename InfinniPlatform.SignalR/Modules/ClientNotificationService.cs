using InfinniPlatform.Sdk.ClientNotification;

using Microsoft.AspNet.SignalR.Hubs;

namespace InfinniPlatform.SignalR.Modules
{
    /// <summary>
    /// Клиент точки обмена для организации оповещения Web-клиентов.
    /// </summary>
    internal sealed class ClientNotificationService : IClientNotificationService
    {
        /// <summary>
        /// Оповестить клиентов.
        /// </summary>
        /// <param name="routingKey">Ключ маршрутизации.</param>
        /// <param name="message">Сообщение.</param>
        public void Notify(string routingKey, object message)
        {
            var context = SignalRGlobalHost.ConnectionManager.GetHubContext<ClientNotificationServiceHub>();

            if (context != null && context.Clients != null && context.Clients.All != null)
            {
                IClientProxy clientProxy = context.Clients.All;
                clientProxy.Invoke(routingKey, message);
            }
        }
    }
}