using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Caching.Factory
{
    internal sealed class CacheFactory : ICacheFactory
    {
        public CacheFactory(IAppConfiguration appConfiguration, ICacheMessageBusFactory cacheMessageBusFactory)
        {
            _cacheSettings = appConfiguration.GetSection<CacheSettings>(CacheSettings.SectionName);
            _redisSettings = appConfiguration.GetSection<RedisSettings>(RedisSettings.SectionName);
            _cacheMessageBusFactory = cacheMessageBusFactory;

            _memoryCache = new Lazy<ICache>(CreateMemoryCache);
            _sharedCache = new Lazy<ICache>(CreateSharedCache);
            _twoLayerCache = new Lazy<ICache>(CreateTwoLayerCache);
        }


        private readonly CacheSettings _cacheSettings;
        private readonly RedisSettings _redisSettings;
        private readonly ICacheMessageBusFactory _cacheMessageBusFactory;

        private readonly Lazy<ICache> _memoryCache;
        private readonly Lazy<ICache> _sharedCache;
        private readonly Lazy<ICache> _twoLayerCache;


        public ICache GetMemoryCache()
        {
            return _memoryCache.Value;
        }

        public ICache GetSharedCache()
        {
            return _sharedCache.Value;
        }

        public ICache GetTwoLayerCache()
        {
            return _twoLayerCache.Value;
        }


        private ICache CreateMemoryCache()
        {
            return new MemoryCacheImpl(_cacheSettings.Name);
        }

        private ICache CreateSharedCache()
        {
            return new RedisCacheImpl(_cacheSettings.Name, _redisSettings.ConnectionString);
        }

        private ICache CreateTwoLayerCache()
        {
            var memoryCache = GetMemoryCache();
            var sharedCache = GetSharedCache();
            var sharedCacheMessageBus = _cacheMessageBusFactory.GetSharedCacheMessageBus();
            var twoLayerCache = new TwoLayerCacheImpl(memoryCache, sharedCache, sharedCacheMessageBus);
            return twoLayerCache;
        }
    }
}