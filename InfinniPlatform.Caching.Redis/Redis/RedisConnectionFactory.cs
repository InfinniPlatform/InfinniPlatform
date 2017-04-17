using StackExchange.Redis;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    ///     Фабрика подключений к Redis.
    /// </summary>
    public class RedisConnectionFactory
    {
        public ConnectionMultiplexer RedisClient { get; }

        public RedisConnectionFactory(RedisConnectionSettings connectionSettings)
        {
            var redisHost = string.IsNullOrEmpty(connectionSettings.Host)
                                ? RedisConnectionSettings.DefaultHost
                                : connectionSettings.Host;

            var redisPort = connectionSettings.Port <= 0
                                ? RedisConnectionSettings.DefaultPort
                                : connectionSettings.Port;

            var writeBufferSize = connectionSettings.WriteBufferSize <= 0
                                      ? RedisConnectionSettings.DefaultWriteBufferSize
                                      : connectionSettings.WriteBufferSize;

            var connectionTimeout = connectionSettings.ConnectionTimeout < 0
                                        ? RedisConnectionSettings.DefaultConnectionTimeout
                                        : connectionSettings.ConnectionTimeout;

            var maxReconnectRetries = connectionSettings.MaxReconnectRetries <= 0
                                          ? RedisConnectionSettings.DefaultMaxReconnectRetries
                                          : connectionSettings.MaxReconnectRetries;

            var configurationOptions = new ConfigurationOptions
                                       {
                                           EndPoints = {{redisHost, redisPort}},
                                           ConnectTimeout = connectionTimeout,
                                           ConnectRetry = maxReconnectRetries,
                                           AbortOnConnectFail = false,
                                           WriteBuffer = writeBufferSize,
                                           Password = connectionSettings.Password,
                                           AllowAdmin = true
                                       };

            RedisClient = ConnectionMultiplexer.Connect(configurationOptions);
        }
    }
}