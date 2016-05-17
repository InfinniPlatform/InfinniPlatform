namespace InfinniPlatform.Sdk.Queues
{
    /// <summary>
    /// Сообщение в очереди.
    /// </summary>
    /// <typeparam name="T">Тип тела сообщения.</typeparam>
    public interface IMessage<out T> : IMessage where T : class
    {
        /// <summary>
        /// Тело сообщения.
        /// </summary>
        T Body { get; }
    }
}