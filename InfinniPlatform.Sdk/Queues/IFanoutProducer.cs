namespace InfinniPlatform.Sdk.Queues
{
    public interface IFanoutProducer
    {
        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Produce(IMessage message);
    }
}