namespace InfinniPlatform.Caching.Factory
{
    /// <summary>
    /// Предоставляет доступ к подсистеме кэширования данных.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Возвращает интерфейс для управления кэшем в памяти.
        /// </summary>
        ICache GetMemoryCache();

        /// <summary>
        /// Возвращает интерфейс для управления распределенным кэшем.
        /// </summary>
        ICache GetSharedCache();

        /// <summary>
        /// Возвращает для управления двухуровневым кэшем (кэшем в памяти и распределенный кэш).
        /// </summary>
        ICache GetTwoLayerCache();
    }
}