using System;
using System.Collections.Generic;

using InfinniPlatform.Cache.Properties;
using InfinniPlatform.Logging;

using Microsoft.Extensions.Logging;

using StackExchange.Redis;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// Реализует интерфейс для управления распределенным кэшем на базе Redis.
    /// </summary>
    [LoggerName(nameof(RedisSharedCache))]
    public class RedisSharedCache : ISharedCache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appOptions">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        /// <param name="logger">Сервис регистрации событий.</param>
        /// <param name="perfLogger">Сервис регистрации длительности выполнения методов.</param>
        public RedisSharedCache(AppOptions appOptions, RedisConnectionFactory connectionFactory, ILogger<RedisSharedCache> logger, IPerformanceLogger<RedisSharedCache> perfLogger)
        {
            _keyspace = appOptions.AppName;
            _connectionFactory = connectionFactory;

            _logger = logger;
            _perfLogger = perfLogger;
        }


        private readonly string _keyspace;
        private readonly RedisConnectionFactory _connectionFactory;

        private readonly ILogger _logger;
        private readonly IPerformanceLogger _perfLogger;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.KeyExists(wk), key, CachingHelpers.PerfLogRedisContainsMethod);
        }

        public string Get(string key)
        {
            string value;

            TryGet(key, out value);

            return value;
        }

        public bool TryGet(string key, out string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheValue = TryExecute((c, wk) => c.StringGet(wk), key, CachingHelpers.PerfLogRedisGetMethod);

            value = cacheValue;

            return !string.IsNullOrEmpty(cacheValue);
        }

        public void Set(string key, string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TryExecute((c, wk) => c.StringSet(wk, value), key, CachingHelpers.PerfLogRedisSetMethod);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.KeyDelete(wk), key, CachingHelpers.PerfLogRedisRemoveMethod);
        }

        public void Clear()
        {
            TryExecute((c, wk) =>
                       {
                           c.Multiplexer.GetServer("localhost:6935").FlushDatabase();
                           return true;
                       },
                CachingHelpers.RedisStarWildcards,
                CachingHelpers.PerfLogRedisClearMethod);
        }


        private T TryExecute<T>(Func<IDatabase, string, T> action, string key, string method)
        {
            var startTime = DateTime.Now;

            Exception error = null;

            var wrappedKey = key.WrapCacheKey(_keyspace);

            try
            {
                var client = _connectionFactory.RedisClient.GetDatabase();

                return action(client, wrappedKey);
            }
            catch (Exception exception)
            {
                error = exception;

                _logger.LogError(Resources.RedisCommandCompletedWithError, error, () => new Dictionary<string, object> { { "method", method }, { "key", key } });

                throw;
            }
            finally
            {
                _perfLogger.Log(method, startTime, error);
            }
        }
    }
}