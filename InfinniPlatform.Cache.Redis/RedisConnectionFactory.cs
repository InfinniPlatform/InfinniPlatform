using StackExchange.Redis;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Фабрика подключений к Redis.
    /// </summary>
    public class RedisConnectionFactory
    {
        public RedisConnectionFactory(RedisSharedCacheOptions options)
        {
            var redisHost = string.IsNullOrEmpty(options.Host)
                                ? RedisSharedCacheOptions.Default.Host
                                : options.Host;

            var redisPort = options.Port <= 0
                                ? RedisSharedCacheOptions.Default.Port
                                : options.Port;

            var writeBufferSize = options.WriteBufferSize <= 0
                                      ? RedisSharedCacheOptions.Default.WriteBufferSize
                                      : options.WriteBufferSize;

            var connectionTimeout = options.ConnectionTimeout < 0
                                        ? RedisSharedCacheOptions.Default.ConnectionTimeout
                                        : options.ConnectionTimeout;

            var maxReconnectRetries = options.MaxReconnectRetries <= 0
                                          ? RedisSharedCacheOptions.Default.MaxReconnectRetries
                                          : options.MaxReconnectRetries;

            var configurationOptions = new ConfigurationOptions
                                       {
                                           EndPoints = {{redisHost, redisPort}},
                                           ConnectTimeout = connectionTimeout,
                                           ConnectRetry = maxReconnectRetries,
                                           AbortOnConnectFail = false,
                                           WriteBuffer = writeBufferSize,
                                           Password = options.Password,
                                           AllowAdmin = true
                                       };

            RedisClient = ConnectionMultiplexer.Connect(configurationOptions);
        }


        public ConnectionMultiplexer RedisClient { get; }
    }
}