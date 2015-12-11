namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Интерфейс шины сообщений.
    /// </summary>
    public interface IMessageBus : IMessageBusManager, IMessageBusPublisher
    {
    }
}