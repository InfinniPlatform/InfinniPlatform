namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Настройки подключения к Redis.
    /// </summary>
    internal sealed class RedisConnectionSettings
    {
        public const string SectionName = "redis";

        public const string DefaultHost = "localhost";

        public const int DefaultPort = 6379;

        public const int DefaultDatabase = 0;

        public const int DefaultReadBufferSize = 512;

        public const int DefaultWriteBufferSize = 512;

        public const int DefaultConnectionTimeout = 0;

        public const int DefaultMaxReconnectRetries = 10;

        public const int DefaultPoolSize = 0;


        public RedisConnectionSettings()
        {
            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            ReadBufferSize = DefaultReadBufferSize;
            WriteBufferSize = DefaultWriteBufferSize;
            ConnectionTimeout = DefaultConnectionTimeout;
            MaxReconnectRetries = DefaultMaxReconnectRetries;
            PoolSize = DefaultPoolSize;
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