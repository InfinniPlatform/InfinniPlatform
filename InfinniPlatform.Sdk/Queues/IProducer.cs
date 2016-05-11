namespace InfinniPlatform.MessageQueue.RabbitMQNew
{
    public interface IProducer
    {
        string QueueName { get; }

        /// <summary>
        /// Публикует сообщение.
        /// </summary>
        /// <param name="message">Сообщение.</param>
        void Produce(byte[] message);
    }
}