namespace InfinniPlatform.Sdk.Queues
{
    public interface IEventingConsumer
    {
        /// <summary>
        /// Обработчик сообщения.
        /// </summary>
        /// <param name="messageBytes">Сообщение в виде массива байт.</param>
        void Consume(byte[] messageBytes);
    }
}