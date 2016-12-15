namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Настройки кэширования.
    /// </summary>
    public sealed class CacheSettings
    {
        public const string SectionName = "cache";

        public const string MemoryCacheKey = "Memory";

        public const string SharedCacheKey = "Shared";


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