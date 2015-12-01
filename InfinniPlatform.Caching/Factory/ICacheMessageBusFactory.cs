namespace InfinniPlatform.Caching.Factory
{
    /// <summary>
    /// Предоставляет доступ к шине сообщений подсистемы кэширования данных.
    /// </summary>
    public interface ICacheMessageBusFactory
    {
        /// <summary>
        /// Возвращает интерфейс шины сообщений для отслеживания изменений в кэша в памяти.
        /// </summary>
        ICacheMessageBus GetMemoryCacheMessageBus();

        /// <summary>
        /// Возвращает интерфейс шины сообщений для отслеживания изменений распределенного кэша.
        /// </summary>
        ICacheMessageBus GetSharedCacheMessageBus();
    }
}