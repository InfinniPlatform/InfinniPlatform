namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Настройки кэширования.
    /// </summary>
    internal sealed class CacheSettings
    {
        public const string SectionName = "cache";

        public const string MemoryCacheKey = "Redis";

        public const string RedisCacheKey = "Redis";

        public const string TwoLayerCacheKey = "TwoLayer";


        public CacheSettings()
        {
            Type = MemoryCacheKey;
        }


        /// <summary>
        /// Тип кэша для приложения.
        /// </summary>
        public string Type { get; set; }
    }
}