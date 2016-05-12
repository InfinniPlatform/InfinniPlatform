using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// Получатель сообщений из очереди по запросу.
    /// </summary>
    public interface IBasicConsumer
    {
        /// <summary>
        /// Получает сообщение из очереди.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        /// <returns>Первое сообщение в очереди или null, если сообщений нет.</returns>
        IMessage Consume<T>(string queueName) where T : class;
    }
}