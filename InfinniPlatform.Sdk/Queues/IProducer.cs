namespace InfinniPlatform.Sdk.Queues
{
    public interface IProducer
    {
        /// <summary>
        /// Публикует сообщение.
        /// </summary>
        /// <param name="message">Сообщение</param>
        /// <param name="queueName">Имя очереди</param>>
        void Produce(IMessage message, string queueName = null);
    }
}