namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Отправитель сообщений в очередь задач.
    /// </summary>
    public interface ITaskProducer
    {
        /// <summary>
        /// Публикует сообщение в очередь задач.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        /// <param name="queueName">Имя очереди.</param>
        void Publish(IMessage message, string queueName = null);
    }
}