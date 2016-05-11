namespace InfinniPlatform.Sdk.Queues
{
    public interface IProducer
    {
        /// <summary>
        /// Публикует сообщение.
        /// </summary>
        /// <param name="queueName">Имя очереди.</param>
        /// <param name="message">Сообщение.</param>
        void Produce(string queueName, IMessage message);
    }
}