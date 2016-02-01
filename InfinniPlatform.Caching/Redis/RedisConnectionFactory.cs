using System;
using System.Text;

using Sider;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Фабрика подключений к Redis.
    /// </summary>
    internal sealed class RedisConnectionFactory
    {
        public RedisConnectionFactory(RedisConnectionSettings connectionSettings)
        {
            var redisHost = string.IsNullOrEmpty(connectionSettings.Host)
                ? RedisConnectionSettings.DefaultHost
                : connectionSettings.Host;

            var redisPort = (connectionSettings.Port <= 0)
                ? RedisConnectionSettings.DefaultPort
                : connectionSettings.Port;

            var redisDatabase = (connectionSettings.Database < 0)
                ? RedisConnectionSettings.DefaultDatabase
                : connectionSettings.Database;

            var readBufferSize = (connectionSettings.ReadBufferSize <= 0)
                ? RedisConnectionSettings.DefaultReadBufferSize
                : connectionSettings.ReadBufferSize;

            var writeBufferSize = (connectionSettings.WriteBufferSize <= 0)
                ? RedisConnectionSettings.DefaultWriteBufferSize
                : connectionSettings.WriteBufferSize;

            var connectionTimeout = (connectionSettings.ConnectionTimeout < 0)
                ? RedisConnectionSettings.DefaultConnectionTimeout
                : connectionSettings.ConnectionTimeout;

            var maxReconnectRetries = (connectionSettings.MaxReconnectRetries <= 0)
                ? RedisConnectionSettings.DefaultMaxReconnectRetries
                : connectionSettings.MaxReconnectRetries;

            // Sider не работает, если PoolSize=1
            var poolSize = (connectionSettings.PoolSize <= 0)
                ? RedisConnectionSettings.DefaultPoolSize
                : Math.Max(connectionSettings.PoolSize, 2);

            var redisSettings = RedisSettings.Build()
                                             .Host(redisHost)
                                             .Port(redisPort)
                                             .ReadBufferSize(readBufferSize)
                                             .WriteBufferSize(writeBufferSize)
                                             .ConnectionTimeout(connectionTimeout)
                                             .MaxReconnectRetries(maxReconnectRetries)
                                             .ReconnectOnIdle(true)
                                             .ReissueCommandsOnReconnect(true)
                                             .ReissuePipelinedCallsOnReconnect(true)
                                             .OverrideEncoding(Encoding.UTF8);

            var redisPassword = connectionSettings.Password;

            if (!string.IsNullOrEmpty(redisPassword))
            {
                _clientInitializer += c => c.Auth(redisPassword);
            }

            if (poolSize > 0)
            {
                _clientsPool = new Lazy<IClientsPool<string>>(() => new RoundRobinPool<string>(redisSettings, poolSize));

                if (redisDatabase > 0)
                {
                    _clientInitializer += c => c.Select(redisDatabase);
                }
            }
            else
            {
                _clientsPool = new Lazy<IClientsPool<string>>(() => new ThreadwisePool<string>(redisSettings, (redisDatabase > 0) ? redisDatabase : default(int?)));
            }
        }


        private readonly Lazy<IClientsPool<string>> _clientsPool;
        private readonly Action<IRedisClient<string>> _clientInitializer;


        public IRedisClient<string> GetClient()
        {
            var client = _clientsPool.Value.GetClient();

            if (_clientInitializer != null)
            {
                _clientInitializer.Invoke(client);
            }

            return client;
        }
    }
}