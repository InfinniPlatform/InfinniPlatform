using System;
using System.Collections.Generic;

using InfinniPlatform.Caching.Properties;
using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Settings;

using Sider;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс для управления распределенным кэшем на базе Redis.
    /// </summary>
    [LoggerName("Redis")]
    internal sealed class RedisCacheImpl : ISharedCache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appEnvironment">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        /// <param name="log">Сервис регистрации событий.</param>
        /// <param name="performanceLog">Сервис регистрации длительности выполнения методов.</param>
        public RedisCacheImpl(IAppEnvironment appEnvironment, RedisConnectionFactory connectionFactory, ILog log, IPerformanceLog performanceLog)
        {
            _keyspace = appEnvironment.Name;
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

            return TryExecute((c, wk) => c.Exists(wk), key, CachingHelpers.PerformanceLogRedisContainsMethod);
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

            var cacheValue = TryExecute((c, wk) => c.Get(wk), key, CachingHelpers.PerformanceLogRedisGetMethod);

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

            TryExecute((c, wk) => c.Set(wk, value), key, CachingHelpers.PerformanceLogRedisSetMethod);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.Del(wk) > 0, key, CachingHelpers.PerformanceLogRedisRemoveMethod);
        }

        public void Clear()
        {
            TryExecute((c, wk) =>
                       {
                           c.Pipeline(pc =>
                                      {
                                          var allKeys = pc.Keys(wk);
                                          pc.Del(allKeys);
                                      });

                           return true;
                       },
                CachingHelpers.RedisStarWildcards,
                CachingHelpers.PerformanceLogRedisClearMethod);
        }


        private T TryExecute<T>(Func<IRedisClient<string>, string, T> action, string key, string method)
        {
            var startTime = DateTime.Now;

            var wrappedKey = key.WrapCacheKey(_keyspace);

            try
            {
                var result = action(_connectionFactory.GetClient(), wrappedKey);

                _performanceLog.Log(method, startTime);

                return result;
            }
            catch (Exception exception)
            {
                var errorContext = new Dictionary<string, object>
                                   {
                                       { "method", method },
                                       { "key", key }
                                   };

                _log.Error(Resources.RedisCommandCompletedWithError, errorContext, exception);

                _performanceLog.Log(method, startTime, exception);

                throw;
            }
        }
    }
}