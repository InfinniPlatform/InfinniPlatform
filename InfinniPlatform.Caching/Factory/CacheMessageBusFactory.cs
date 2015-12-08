using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Sdk.Environment.Settings;

namespace InfinniPlatform.Caching.Factory
{
    internal sealed class CacheMessageBusFactory : ICacheMessageBusFactory
    {
        public CacheMessageBusFactory(IAppConfiguration appConfiguration)
        {
            var cacheSettings = appConfiguration.GetSection<CacheSettings>(CacheSettings.SectionName);

            ICacheMessageBus cacheMessageBus;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCackeKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCackeKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisSettings = appConfiguration.GetSection<RedisSettings>(RedisSettings.SectionName);

                cacheMessageBus = new RedisCacheMessageBusImpl(cacheSettings.Name, redisSettings.ConnectionString);
            }
            else
            {
                cacheMessageBus = new MemoryCacheMessageBusImpl();
            }

            _cacheMessageBus = cacheMessageBus;
        }


        private readonly ICacheMessageBus _cacheMessageBus;


        public ICacheMessageBus CreateCacheMessageBus()
        {
            return _cacheMessageBus;
        }
    }
}