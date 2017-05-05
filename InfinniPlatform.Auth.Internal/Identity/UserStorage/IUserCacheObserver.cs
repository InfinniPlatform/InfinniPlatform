using System.Threading.Tasks;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Auth.Identity.UserStorage
{
    /// <summary>
    /// Интерфейс синхронизации кэша пользователей через очередь сообщений.
    /// </summary>
    internal interface IUserCacheObserver
    {
        /// <summary>
        /// Обрабатывает сообщение из очереди.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        Task ProcessMessage(Message<string> message);
    }
}