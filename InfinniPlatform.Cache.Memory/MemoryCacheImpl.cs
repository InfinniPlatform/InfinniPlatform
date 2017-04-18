using System;
using Microsoft.Extensions.Caching.Memory;
using IMemoryCache = InfinniPlatform.Cache.Abstractions.IMemoryCache;

namespace InfinniPlatform.Cache.Memory
{
    /// <summary>
    ///     Реализует интерфейс для управления кэшем в памяти.
    /// </summary>
    public class MemoryCacheImpl : IMemoryCache, IDisposable
    {
        private readonly MemoryCache _cache;

        /// <summary>
        ///     Конструктор.
        /// </summary>
        public MemoryCacheImpl()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }

        public void Dispose()
        {
            _cache.Dispose();
        }


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.TryGetValue(key, out object value);
        }

        public string Get(string key)
        {
            TryGet(key, out string value);

            return value;
        }

        public bool TryGet(string key, out string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            value = _cache.Get<string>(key);

            return value != null;
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

            _cache.Set(key, value, new MemoryCacheEntryOptions());
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            try
            {
                _cache.Remove(key);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}