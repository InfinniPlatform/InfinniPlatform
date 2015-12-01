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
            _cacheSettings = appConfiguration.GetSection<CacheSettings>(CacheSettings.SectionName);
            _redisSettings = appConfiguration.GetSection<RedisSettings>(RedisSettings.SectionName);

            _memoryCacheMessageBus = new Lazy<ICacheMessageBus>(CreateMemoryCacheMessageBus);
            _sharedCacheMessageBus = new Lazy<ICacheMessageBus>(CreateSharedCacheMessageBus);
        }


        private readonly CacheSettings _cacheSettings;
        private readonly RedisSettings _redisSettings;

        private readonly Lazy<ICacheMessageBus> _memoryCacheMessageBus;
        private readonly Lazy<ICacheMessageBus> _sharedCacheMessageBus;


        public ICacheMessageBus GetMemoryCacheMessageBus()
        {
            return _memoryCacheMessageBus.Value;
        }

        public ICacheMessageBus GetSharedCacheMessageBus()
        {
            return _sharedCacheMessageBus.Value;
        }


        private static ICacheMessageBus CreateMemoryCacheMessageBus()
        {
            return new MemoryCacheMessageBusImpl();
        }

        private ICacheMessageBus CreateSharedCacheMessageBus()
        {
            return new RedisCacheMessageBusImpl(_cacheSettings.Name, _redisSettings.ConnectionString);
        }
    }
}