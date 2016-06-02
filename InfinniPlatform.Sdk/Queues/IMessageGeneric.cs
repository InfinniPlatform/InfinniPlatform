namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Сообщение в очереди.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public interface IMessage<out T> : IMessage
    {
        /// <summary>
        /// Тело сообщения.
        /// </summary>
        T Body { get; }
    }
}