using System;

using Sider;

namespace InfinniPlatform.Caching.Redis
{
    /// <summary>
    /// Реализует интерфейс для управления распределенным кэшем на базе Redis.
    /// </summary>
    internal sealed class RedisCacheImpl : ICache
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="name">Пространство имен для ключей.</param>
        /// <param name="connectionFactory">Фабрика подключений к Redis.</param>
        public RedisCacheImpl(string name, RedisConnectionFactory connectionFactory)
        {
            _name = name;
            _connectionFactory = connectionFactory;
        }


        private readonly string _name;
        private readonly RedisConnectionFactory _connectionFactory;


        public bool Contains(string key)
        {
            Logging.Logger.Log.Info($"REDIS: Contains({key})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.Exists(wk), key);
        }

        public string Get(string key)
        {
            Logging.Logger.Log.Info($"REDIS: Get({key})");

            string value;

            TryGet(key, out value);

            return value;
        }

        public bool TryGet(string key, out string value)
        {
            Logging.Logger.Log.Info($"REDIS: TryGet({key})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheValue = TryExecute((c, wk) => c.Get(wk), key);

            value = cacheValue;

            return !string.IsNullOrEmpty(cacheValue);
        }

        public void Set(string key, string value)
        {
            Logging.Logger.Log.Info($"REDIS: Set({key}, {value})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            TryExecute((c, wk) => c.Set(wk, value), key);
        }

        public bool Remove(string key)
        {
            Logging.Logger.Log.Info($"REDIS: Remove({key})");

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return TryExecute((c, wk) => c.Del(wk) > 0, key);
        }

        public void Clear()
        {
            Logging.Logger.Log.Info($"REDIS: Clear()");

            TryExecute((c, wk) =>
                       {
                           c.Pipeline(pc =>
                                      {
                                          var allKeys = pc.Keys(wk);
                                          pc.Del(allKeys);
                                      });

                           return true;
                       }, "*");
        }


        private T TryExecute<T>(Func<IRedisClient<string>, string, T> action, string key)
        {
            var wrappedKey = key.WrapCacheKey(_name);

            return action(_connectionFactory.GetClient(), wrappedKey);
        }
    }
}