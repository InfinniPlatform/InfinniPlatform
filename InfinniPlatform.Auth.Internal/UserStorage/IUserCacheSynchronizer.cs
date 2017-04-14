using System.Threading.Tasks;
using InfinniPlatform.MessageQueue.Abstractions;

namespace InfinniPlatform.Auth.Internal.UserStorage
{
    /// <summary>
    /// Интерфейс синхронизации кэша пользователей через очередь сообщений.
    /// </summary>
    internal interface IUserCacheSynchronizer
    {
        /// <summary>
        /// Обрабатывает сообщение из очереди.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        Task ProcessMessage(Message<string> message);
    }
}