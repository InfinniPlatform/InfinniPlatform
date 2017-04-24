using System.Threading.Tasks;

using InfinniPlatform.Dynamic;

namespace InfinniPlatform.MessageQueue
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
        void PublishDynamic(DynamicDocument messageBody, string queueName);

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
        Task PublishDynamicAsync(DynamicDocument messageBody, string queueName);
    }
}