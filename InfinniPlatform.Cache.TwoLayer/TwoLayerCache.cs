using System;

namespace InfinniPlatform.Cache
{
    public class TwoLayerCache : ITwoLayerCache
    {
        public TwoLayerCache(IInMemoryCacheFactory inMemoryCacheFactory, ISharedCacheFactory sharedCacheFactory, ITwoLayerCacheStateObserver сacheStateObserver)
        {
            _inMemoryCache = inMemoryCacheFactory.Create();
            _sharedCache = sharedCacheFactory.Create();
            _сacheStateObserver = сacheStateObserver;
        }


        private readonly IInMemoryCache _inMemoryCache;
        private readonly ISharedCache _sharedCache;
        private readonly ITwoLayerCacheStateObserver _сacheStateObserver;


        public bool Contains(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            return _inMemoryCache.Contains(key) || _sharedCache.Contains(key);
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

            var exists = false;

            if (!_inMemoryCache.TryGet(key, out value))
            {
                if (!_sharedCache.TryGet(key, out value))
                {
                    value = null;
                }
                else
                {
                    exists = true;

                    _inMemoryCache.Set(key, value);
                }
            }
            else
            {
                exists = true;
            }

            return exists;
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

            _inMemoryCache.Set(key, value);
            _sharedCache.Set(key, value);

            NotifyOnKeyChangedAsync(key);
        }

        public bool Remove(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullException(nameof(key));
            }

            var deleted = false;

            try
            {
                try
                {
                    deleted |= _inMemoryCache.Remove(key);
                }
                finally
                {
                    deleted |= _sharedCache.Remove(key);
                }
            }
            finally
            {
                if (deleted)
                {
                    NotifyOnKeyChangedAsync(key);
                }
            }

            return deleted;
        }


        private void NotifyOnKeyChangedAsync(string key)
        {
            _сacheStateObserver.OnResetKey(key);
        }
    }
}