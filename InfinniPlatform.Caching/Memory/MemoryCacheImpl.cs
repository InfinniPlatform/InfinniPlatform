using System;
using System.Linq;
using System.Runtime.Caching;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Реализует интерфейс для управления кэшем в памяти.
    /// </summary>
    internal sealed class MemoryCacheImpl : ICache, IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="keyspace">Пространство имен для ключей.</param>
        /// <exception cref="ArgumentNullException"></exception>
        public MemoryCacheImpl(string keyspace)
        {
            if (string.IsNullOrEmpty(keyspace))
            {
                throw new ArgumentNullException(nameof(keyspace));
            }

            _cache = new MemoryCache(keyspace);
        }


        private readonly MemoryCache _cache;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.Contains(key);
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

            value = (string)_cache.Get(key);

            return (value != null);
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

            _cache.Set(key, value, new CacheItemPolicy());
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return (_cache.Remove(key) != null);
        }

        public void Clear()
        {
            foreach (var item in _cache.ToArray())
            {
                _cache.Remove(item.Key);
            }
        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}