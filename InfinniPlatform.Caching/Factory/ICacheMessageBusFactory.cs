namespace InfinniPlatform.Caching.Factory
{
    /// <summary>
    /// Предоставляет доступ к шине сообщений подсистемы кэширования данных.
    /// </summary>
    public interface ICacheMessageBusFactory
    {
        /// <summary>
        /// Возвращает интерфейс шины сообщений для отслеживания изменений кэша.
        /// </summary>
        ICacheMessageBus CreateCacheMessageBus();
    }
}