namespace InfinniPlatform.MessageQueue.RabbitMQNew
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
        string Consume();
    }
}