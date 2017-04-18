using StackExchange.Redis;

namespace InfinniPlatform.Cache.Redis
{
    /// <summary>
    ///     Фабрика подключений к Redis.
    /// </summary>
    public class RedisConnectionFactory
    {
        public ConnectionMultiplexer RedisClient { get; }

        public RedisConnectionFactory(RedisCacheOptions cacheOptions)
        {
            var redisHost = string.IsNullOrEmpty(cacheOptions.Host)
                                ? RedisCacheOptions.Default.Host
                                : cacheOptions.Host;

            var redisPort = cacheOptions.Port <= 0
                                ? RedisCacheOptions.Default.Port
                                : cacheOptions.Port;

            var writeBufferSize = cacheOptions.WriteBufferSize <= 0
                                      ? RedisCacheOptions.Default.WriteBufferSize
                                      : cacheOptions.WriteBufferSize;

            var connectionTimeout = cacheOptions.ConnectionTimeout < 0
                                        ? RedisCacheOptions.Default.ConnectionTimeout
                                        : cacheOptions.ConnectionTimeout;

            var maxReconnectRetries = cacheOptions.MaxReconnectRetries <= 0
                                          ? RedisCacheOptions.Default.MaxReconnectRetries
                                          : cacheOptions.MaxReconnectRetries;

            var configurationOptions = new ConfigurationOptions
                                       {
                                           EndPoints = {{redisHost, redisPort}},
                                           ConnectTimeout = connectionTimeout,
                                           ConnectRetry = maxReconnectRetries,
                                           AbortOnConnectFail = false,
                                           WriteBuffer = writeBufferSize,
                                           Password = cacheOptions.Password,
                                           AllowAdmin = true
                                       };

            RedisClient = ConnectionMultiplexer.Connect(configurationOptions);
        }
    }
}