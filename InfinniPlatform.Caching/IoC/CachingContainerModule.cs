using System;

using InfinniPlatform.Caching.Diagnostics;
using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.Session;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.Diagnostics;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.MessageQueue.Contract;
using InfinniPlatform.Sdk.Session;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // НАСТРОЙКИ

            builder.RegisterFactory(GetCacheSettings)
                   .As<CacheSettings>()
                   .SingleInstance();

            builder.RegisterFactory(GetRedisConnectionSettings)
                   .As<RedisConnectionSettings>()
                   .SingleInstance();

            // ПОДКЛЮЧЕНИЕ К Redis

            builder.RegisterType<RedisConnectionFactory>()
                   .AsSelf()
                   .InstancePerDependency();

            // РЕАЛИЗАЦИИ КЭША

            builder.RegisterType<MemoryCacheImpl>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RedisCacheImpl>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<TwoLayerCacheImpl>()
                   .As<ICache>()
                   .As<ICacheSynchronizer>()
                   .SingleInstance();

            builder.RegisterType<TwoLayerCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<CachingMessageConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();

            builder.RegisterType<MemoryCacheImpl>()
                   .As<IMemoryCache>()
                   .SingleInstance();

            builder.RegisterFactory(GetSharedCache)
                   .As<ISharedCache>()
                   .SingleInstance();

            // СЕССИЯ ПОЛЬЗОВАТЕЛЯ

            builder.RegisterType<SessionManager>()
                   .As<ISessionManager>()
                   .SingleInstance();

            // ДИАГНОСТИКА

            builder.RegisterType<CachingStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }


        private static CacheSettings GetCacheSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>()
                           .GetSection<CacheSettings>(CacheSettings.SectionName);
        }


        private static RedisConnectionSettings GetRedisConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>()
                           .GetSection<RedisConnectionSettings>(RedisConnectionSettings.SectionName);
        }

        private static ISharedCache GetSharedCache(IContainerResolver resolver)
        {
            var cacheSettings = resolver.Resolve<CacheSettings>();

            if (cacheSettings.Type == CacheSettings.MemoryCacheKey)
            {
                return new NullSharedCacheImpl();
            }

            var redisCacheFactory = resolver.Resolve<Func<IAppEnvironment, RedisCacheImpl>>();
            ISharedCache cache = redisCacheFactory(resolver.Resolve<IAppEnvironment>());

            return cache;
        }
    }
}