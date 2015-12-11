namespace InfinniPlatform.Caching
{
    /// <summary>
    /// Настройки кэширования.
    /// </summary>
    internal sealed class CacheSettings
    {
        public const string SectionName = "cache";

        public const string RedisCackeKey = "Redis";

        public const string TwoLayerCackeKey = "TwoLayer";


        public CacheSettings()
        {
            Name = "InfinniPlatform";
            Type = "Memory";
        }


        /// <summary>
        /// Имя кэша для приложения.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип кэша для приложения.
        /// </summary>
        public string Type { get; set; }
    }
}