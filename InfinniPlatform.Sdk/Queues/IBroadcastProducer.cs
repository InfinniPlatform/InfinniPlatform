namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Отправитель сообщений в широковещательную очередь.
    /// </summary>
    public interface IBroadcastProducer
    {
        /// <summary>
        /// Публикует широковещательные сообщения.
        /// </summary>
        /// <param name="message">Сообщение</param>
        void Publish(IMessage message);
    }
}