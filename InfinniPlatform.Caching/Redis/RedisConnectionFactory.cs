using System;
using StackExchange.Redis;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    ///     Фабрика подключений к Redis.
    /// </summary>
    internal sealed class RedisConnectionFactory
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

            var redisDatabase = connectionSettings.Database < 0
                                    ? RedisConnectionSettings.DefaultDatabase
                                    : connectionSettings.Database;

            var readBufferSize = connectionSettings.ReadBufferSize <= 0
                                     ? RedisConnectionSettings.DefaultReadBufferSize
                                     : connectionSettings.ReadBufferSize;

            var writeBufferSize = connectionSettings.WriteBufferSize <= 0
                                      ? RedisConnectionSettings.DefaultWriteBufferSize
                                      : connectionSettings.WriteBufferSize;

            var connectionTimeout = connectionSettings.ConnectionTimeout < 0
                                        ? RedisConnectionSettings.DefaultConnectionTimeout
                                        : connectionSettings.ConnectionTimeout;

            var maxReconnectRetries = connectionSettings.MaxReconnectRetries <= 0
                                          ? RedisConnectionSettings.DefaultMaxReconnectRetries
                                          : connectionSettings.MaxReconnectRetries;

            // Sider не работает, если PoolSize=1
            var poolSize = connectionSettings.PoolSize <= 0
                               ? RedisConnectionSettings.DefaultPoolSize
                               : Math.Max(connectionSettings.PoolSize, 2);

            //TODO Check unused settings.
            //            var redisSettings = RedisSettings.Build()
            //                                             .Host(redisHost)
            //                                             .Port(redisPort)
            //                                             .ReadBufferSize(readBufferSize)
            //                                             .WriteBufferSize(writeBufferSize)
            //                                             .ConnectionTimeout(connectionTimeout)
            //                                             .MaxReconnectRetries(maxReconnectRetries)
            //                                             .ReconnectOnIdle(true)
            //                                             .ReissueCommandsOnReconnect(true)
            //                                             .ReissuePipelinedCallsOnReconnect(true)
            //                                             .OverrideEncoding(Encoding.UTF8);


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