using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// Получатель сообщений из очереди по запросу.
    /// </summary>
    public interface IBasicConsumer
    {
        string QueueName { get; }

        /// <summary>
        /// Получает сообщение из очереди.
        /// </summary>
        /// <returns>Первое сообщение в очереди или null, если сообщений нет.</returns>
        IMessage Consume<T>() where T : class;
    }
}