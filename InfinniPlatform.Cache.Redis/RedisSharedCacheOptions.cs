namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Redis shared cache configuration options.
    /// </summary>
    public class RedisSharedCacheOptions : IOptions
    {
        /// <inheritdoc />
        public string SectionName => "redisSharedCache";

        /// <summary>
        /// Default instance of <see cref="RedisSharedCacheOptions" />.
        /// </summary>
        public static readonly RedisSharedCacheOptions Default = new RedisSharedCacheOptions();


        /// <summary>
        /// Initializes a new instance of <see cref="RedisSharedCacheOptions" />.
        /// </summary>
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
        /// Redis server address.
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// redis server port.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Database number.
        /// </summary>
        public int Database { get; set; }

        /// <summary>
        /// Password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Read buffer size in bytes.
        /// </summary>
        public int ReadBufferSize { get; set; }

        /// <summary>
        /// Write buffer size in bytes.
        /// </summary>
        public int WriteBufferSize { get; set; }

        /// <summary>
        /// Connection timeout in milliseconds.
        /// </summary>
        public int ConnectionTimeout { get; set; }

        /// <summary>
        /// Maximum number of Reconnect retries.
        /// </summary>
        public int MaxReconnectRetries { get; set; }

        /// <summary>
        /// Connection pool size.
        /// </summary>
        public int PoolSize { get; set; }
    }
}