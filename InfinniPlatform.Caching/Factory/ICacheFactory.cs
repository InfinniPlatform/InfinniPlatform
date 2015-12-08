namespace InfinniPlatform.Caching.Factory
{
    /// <summary>
    /// Предоставляет доступ к подсистеме кэширования данных.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        /// Возвращает интерфейс для управления кэшем.
        /// </summary>
        ICache CreateCache();
    }
}