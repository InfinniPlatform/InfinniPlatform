namespace InfinniPlatform.Caching.Factory
{
    /// <summary>
    /// Настройки подключения к Redis.
    /// </summary>
    internal sealed class RedisSettings
    {
        public const string SectionName = "redis";


        public RedisSettings()
        {
            ConnectionString = "localhost";
        }


        /// <summary>
        /// Строка подключения.
        /// </summary>
        public string ConnectionString { get; set; }
    }
}