using System.Threading.Tasks;

namespace InfinniPlatform.PushNotification.Contract
{
    /// <summary>
    /// Сервис для PUSH-уведомлений Web-клиентов.
    /// </summary>
    public interface IPushNotificationService
    {
        /// <summary>
        /// Отправляет сообщение всем клиентам.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="message">Сообщение.</param>
        Task NotifyAll(string messageType, object message);

        /// <summary>
        /// Отправляет сообщение всем клиентам указанной организации.
        /// </summary>
        /// <param name="tenantId">Идентификатор организации.</param>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="message">Сообщение.</param>
        Task NotifyTenant(string tenantId, string messageType, object message);

        /// <summary>
        /// Отправляет сообщение всем клиентам текущей организации.
        /// </summary>
        /// <param name="messageType">Тип сообщения.</param>
        /// <param name="message">Сообщение.</param>
        Task NotifyCurrentTenant(string messageType, object message);
    }
}