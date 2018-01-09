using System;

using Microsoft.Extensions.Caching.Memory;

namespace InfinniPlatform.Cache
{
    /// <summary>
    /// In-memory cache implementation.
    /// </summary>
    public class InMemoryCache : IInMemoryCache, IDisposable
    {
        /// <summary>
        /// Initializes a new instance of <see cref="InMemoryCache" />.
        /// </summary>
        public InMemoryCache()
        {
            _cache = new MemoryCache(new MemoryCacheOptions());
        }


        private readonly MemoryCache _cache;


        /// <inheritdoc />
        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _cache.TryGetValue(key, out var _);
        }

        /// <inheritdoc />
        public string Get(string key)
        {
            TryGet(key, out string value);

            return value;
        }

        /// <inheritdoc />
        public bool TryGet(string key, out string value)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            value = _cache.Get<string>(key);

            return value != null;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
        public void Dispose()
        {
            _cache.Dispose();
        }
    }
}