using System.Threading.Tasks;

using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.Sdk.Cache
{
    /// <summary>
    /// Интерфейс синхронизации кэша через очередь сообщений.
    /// </summary>
    public interface ICacheSynchronizer
    {
        /// <summary>
        /// Обрабатывает сообщение из очереди.
        /// </summary>
        /// <param name="message">Сообщение из очереди.</param>
        Task ProcessMessage(Message<string> message);

        /// <summary>
        /// Оповещает получателей об изменении значения по ключу.
        /// </summary>
        /// <param name="key">Ключ.</param>
        void NotifyOnKeyChanged(string key);
    }
}