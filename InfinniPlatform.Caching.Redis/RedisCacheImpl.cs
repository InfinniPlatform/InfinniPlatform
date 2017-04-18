using System;
using System.Collections.Generic;
using InfinniPlatform.Caching.Abstractions;
using InfinniPlatform.Caching.Redis.Properties;
using InfinniPlatform.Core.Abstractions.Logging;
using InfinniPlatform.Core.Abstractions.Settings;

using StackExchange.Redis;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс для управления распределенным кэшем на базе Redis.
    /// </summary>
    [LoggerName("Redis")]
    public class RedisCacheImpl : ISharedCache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appOptions">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        /// <param name="log">Сервис регистрации событий.</param>
        /// <param name="performanceLog">Сервис регистрации длительности выполнения методов.</param>
        public RedisCacheImpl(AppOptions appOptions, RedisConnectionFactory connectionFactory, ILog log, IPerformanceLog performanceLog)
        {
            _keyspace = appOptions.AppName;
            _connectionFactory = connectionFactory;

            _log = log;
            _performanceLog = performanceLog;
        }


        private readonly string _keyspace;
        private readonly RedisConnectionFactory _connectionFactory;

        private readonly ILog _log;
        private readonly IPerformanceLog _performanceLog;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.KeyExists(wk), key, CachingHelpers.PerformanceLogRedisContainsMethod);
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

            var cacheValue = TryExecute((c, wk) => c.StringGet(wk), key, CachingHelpers.PerformanceLogRedisGetMethod);

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

            TryExecute((c, wk) => c.StringSet(wk, value), key, CachingHelpers.PerformanceLogRedisSetMethod);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.KeyDelete(wk), key, CachingHelpers.PerformanceLogRedisRemoveMethod);
        }

        public void Clear()
        {
            TryExecute((c, wk) =>
                       {
                           c.Multiplexer.GetServer("localhost:6935").FlushDatabase();
                           return true;
                       },
                CachingHelpers.RedisStarWildcards,
                CachingHelpers.PerformanceLogRedisClearMethod);
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

                _log.Error(Resources.RedisCommandCompletedWithError, error, () => new Dictionary<string, object> { { "method", method }, { "key", key } });

                throw;
            }
            finally
            {
                _performanceLog.Log(method, startTime, error);
            }
        }
    }
}