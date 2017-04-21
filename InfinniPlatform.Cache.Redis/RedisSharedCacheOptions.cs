namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Настройки распределенного кэша на основе Redis.
    /// </summary>
    public class RedisSharedCacheOptions
    {
        public const string SectionName = "redisSharedCache";

        public static readonly RedisSharedCacheOptions Default = new RedisSharedCacheOptions();


        public RedisSharedCacheOptions()
        {
            Host = "localhost";
            Port = 6379;
            Database = 0;
            ReadBufferSize = 512;
            WriteBufferSize = 512;
            ConnectionTimeout = 1000;
            MaxReconnectRetries = 10;
            PoolSize = 0;
        }


        /// <summary>
        /// Адрес или доменное имя.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// Номер порта.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Номер базы данных.
        /// </summary>
        public int Database { get; set; }

        /// <summary>
        /// Пароль подключения.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Размер буфера чтения.
        /// </summary>
        public int ReadBufferSize { get; set; }

        /// <summary>
        /// Размер буфера записи.
        /// </summary>
        public int WriteBufferSize { get; set; }

        /// <summary>
        /// Таймаут подключения.
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// Количество попыток подключения.
        /// </summary>
        public int MaxReconnectRetries { get; set; }

        /// <summary>
        /// Размер пула подключений.
        /// </summary>
        public int PoolSize { get; set; }
    }
}