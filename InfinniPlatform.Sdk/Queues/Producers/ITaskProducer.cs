using System.Threading.Tasks;

using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Sdk.Queues.Producers
{
    /// <summary>
    /// Отправитель сообщений в очередь задач.
    /// </summary>
    public interface ITaskProducer
    {
        /// <summary>
        /// Публикует сообщение в очередь задач.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди. Если не указано - используется полное наименование типа тела сообщения.</param>
        void Publish<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует сообщение в очередь задач.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди.</param>
        void PublishDynamic(DynamicWrapper messageBody, string queueName);

        /// <summary>
        /// Публикует сообщение в очередь задач.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди. Если не указано - используется полное наименование типа тела сообщения.</param>
        Task PublishAsync<T>(T messageBody, string queueName = null);

        /// <summary>
        /// Публикует сообщение в очередь задач.
        /// </summary>
        /// <param name="messageBody">Тело сообщения.</param>
        /// <param name="queueName">Имя очереди.</param>
        Task PublishDynamicAsync(DynamicWrapper messageBody, string queueName);
    }
}