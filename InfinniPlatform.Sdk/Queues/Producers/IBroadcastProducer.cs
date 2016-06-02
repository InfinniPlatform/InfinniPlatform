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
        /// <param name="messageBody">Сообщение</param>
        void Publish<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Сообщение</param>
        void PublishDynamic(DynamicWrapper messageBody, string queueName);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Сообщение</param>
        Task PublishAsync<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Сообщение</param>
        Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName);
    }
}