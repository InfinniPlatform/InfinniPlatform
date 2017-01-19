using System;
using Microsoft.Extensions.Caching.Memory;

namespace InfinniPlatform.Caching.Memory
{
    /// <summary>
    /// Реализует интерфейс для управления кэшем в памяти.
    /// </summary>
    internal sealed class MemoryCacheImpl : IMemoryCache, IDisposable
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="appEnvironment">Пространство имен для ключей.</param>
        public MemoryCacheImpl()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }


        private readonly MemoryCache _cache;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            object value
                ;
            return _cache.TryGetValue(key,out value);
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

            value = _cache.Get<string>(key);

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

            _cache.Set(key, value, new MemoryCacheEntryOptions());
        }

        public void Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            _cache.Remove(key);
        }

        // TODO Cant get all items from cache Microsoft.Extensions.Caching.Memory.MemoryCache
//        public void Clear()
//        {
//            foreach (var item in _cache.ToArray())
//            {
//                _cache.Remove(item.Key);
//            }
//        }

        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}