using System.Threading.Tasks;

namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Интерфейс публикации сообщений в шину.
    /// </summary>
    public interface IMessageBusPublisher
    {
        /// <summary>
        /// Публикует сообщение.
        /// </summary>
        /// <param name="key">Ключ.</param>
        /// <param name="value">Значение.</param>
        Task Publish(string key, string value);
    }
}