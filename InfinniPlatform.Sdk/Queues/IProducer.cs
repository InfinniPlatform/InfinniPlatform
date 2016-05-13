namespace InfinniPlatform.Sdk.Queues
{
    public interface IProducer
    {
        /// <summary>
        /// Публикует сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void Produce(IMessage message, string queueName = null);
    }
}