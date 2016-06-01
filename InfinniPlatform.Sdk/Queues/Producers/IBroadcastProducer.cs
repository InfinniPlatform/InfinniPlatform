using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Queues.Producers
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
        void Publish<T>(T message, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        void PublishDynamic(DynamicWrapper message, string queueName);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        Task PublishAsync<T>(T message, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        Task PublishDynamicAsync(DynamicWrapper message, string queueName);
    }
}