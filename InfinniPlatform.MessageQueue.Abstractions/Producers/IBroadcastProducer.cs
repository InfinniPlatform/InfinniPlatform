using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue.Abstractions.Producers
{
    /// <summary>
    /// Отправитель сообщений в широковещательную очередь.
    /// </summary>
    public interface IBroadcastProducer
    {
        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди. Если не указано - используется полное наименование типа тела сообщения.</param>
        void Publish<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди.</param>
        void PublishDynamic(DynamicWrapper messageBody, string queueName);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// Если не указано имя очереди - используется полное наименование типа тела сообщения.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди. Если не указано - используется полное наименование типа тела сообщения.</param>
        Task PublishAsync<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди.</param>
        Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName);
    }
}