using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Caching.Factory
{
    internal sealed class CacheFactory : ICacheFactory
    {
        public CacheFactory(IAppConfiguration appConfiguration, ICacheMessageBus cacheMessageBus)
        {
            var cacheSettings = appConfiguration.GetSection<CacheSettings>(CacheSettings.SectionName);

            ICache cache;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCackeKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisSettings = appConfiguration.GetSection<RedisSettings>(RedisSettings.SectionName);
                var redisCache = new RedisCacheImpl(cacheSettings.Name, redisSettings.ConnectionString);
                cache = redisCache;
            }
            else if (string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCackeKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisSettings = appConfiguration.GetSection<RedisSettings>(RedisSettings.SectionName);
                var memoryCache = new MemoryCacheImpl(cacheSettings.Name);
                var redisCache = new RedisCacheImpl(cacheSettings.Name, redisSettings.ConnectionString);
                var twoLayerCache = new TwoLayerCacheImpl(memoryCache, redisCache, cacheMessageBus);
                cache = twoLayerCache;
            }
            else
            {
                var memoryCache = new MemoryCacheImpl(cacheSettings.Name);
                cache = memoryCache;
            }

            _cache = cache;
        }


        private readonly ICache _cache;


        public ICache CreateCache()
        {
            return _cache;
        }
    }
}