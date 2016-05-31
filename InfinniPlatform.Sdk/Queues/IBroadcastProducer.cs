using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Отправитель сообщений в широковещательную очередь.
    /// </summary>
    public interface IBroadcastProducer
    {
        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Publish<T>(T message) where T : class;

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Publish(DynamicWrapper message);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        Task PublishAsync<T>(T message) where T : class;

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        Task PublishAsync(DynamicWrapper message);
    }
}