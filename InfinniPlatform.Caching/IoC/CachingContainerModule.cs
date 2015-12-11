using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.Session;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Log;
using InfinniPlatform.Sdk.Environment.Settings;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Настройки кэширования
            builder.RegisterFactory(GetCacheSettings)
                   .As<CacheSettings>()
                   .SingleInstance();

            // Настройки подключения к Redis
            builder.RegisterFactory(GetRedisConnectionSettings)
                   .As<RedisConnectionSettings>()
                   .SingleInstance();

            // Фабрика подключений к Redis
            builder.RegisterType<RedisConnectionFactory>()
                   .AsSelf()
                   .SingleInstance();

            // Подсистема кэширования данных
            builder.RegisterFactory(GetCache)
                   .As<ICache>()
                   .SingleInstance();

            // Шина сообщений подсистемы кэширования данных
            builder.RegisterFactory(GetCacheMessageBus)
                   .As<ICacheMessageBus>()
                   .SingleInstance();

            // Менеджер данных сессии пользователя
            builder.RegisterType<SessionManager>()
                   .As<ISessionManager>()
                   .SingleInstance();
        }


        private static CacheSettings GetCacheSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<CacheSettings>(CacheSettings.SectionName);
        }


        private static RedisConnectionSettings GetRedisConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<RedisConnectionSettings>(RedisConnectionSettings.SectionName);
        }


        private static ICache GetCache(IContainerResolver resolver)
        {
            var cacheSettings = resolver.Resolve<CacheSettings>();

            ICache cache;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var log = resolver.Resolve<ILog>();
                var performanceLog = resolver.Resolve<IPerformanceLog>();

                var redisConnectionFactory = resolver.Resolve<RedisConnectionFactory>();
                var redisCache = new RedisCacheImpl(cacheSettings.Name, redisConnectionFactory, log, performanceLog);
                cache = redisCache;
            }
            else if (string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var log = resolver.Resolve<ILog>();
                var performanceLog = resolver.Resolve<IPerformanceLog>();

                var redisConnectionFactory = resolver.Resolve<RedisConnectionFactory>();
                var memoryCache = new MemoryCacheImpl(cacheSettings.Name);
                var redisCache = new RedisCacheImpl(cacheSettings.Name, redisConnectionFactory, log, performanceLog);
                var redisCacheMessageBus = resolver.Resolve<ICacheMessageBus>();
                var twoLayerCache = new TwoLayerCacheImpl(memoryCache, redisCache, redisCacheMessageBus);
                cache = twoLayerCache;
            }
            else
            {
                var memoryCache = new MemoryCacheImpl(cacheSettings.Name);
                cache = memoryCache;
            }

            return cache;
        }


        private static ICacheMessageBus GetCacheMessageBus(IContainerResolver resolver)
        {
            var cacheSettings = resolver.Resolve<CacheSettings>();

            ICacheMessageBus cacheMessageBus;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCacheKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var log = resolver.Resolve<ILog>();
                var performanceLog = resolver.Resolve<IPerformanceLog>();

                var redisConnectionFactory = resolver.Resolve<RedisConnectionFactory>();
                cacheMessageBus = new RedisCacheMessageBusImpl(cacheSettings.Name, redisConnectionFactory, log, performanceLog);
            }
            else
            {
                cacheMessageBus = new MemoryCacheMessageBusImpl();
            }

            return cacheMessageBus;
        }
    }
}