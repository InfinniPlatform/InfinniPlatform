using InfinniPlatform.Sdk.Queues;

namespace InfinniPlatform.MessageQueue.RabbitMq
{
    /// <summary>
    /// ѕолучатель сообщений из очереди по запросу.
    /// </summary>
    public interface IBasicConsumer
    {
        string QueueName { get; }

        /// <summary>
        /// ѕолучает сообщение из очереди.
        /// </summary>
        /// <returns>ѕервое сообщение в очереди или null, если сообщений нет.</returns>
        IMessage Consume<T>() where T : class;
    }
}