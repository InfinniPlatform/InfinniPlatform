using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.Session;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Session;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<MessageBusSubscriptions>()
                   .AsSelf()
                   .SingleInstance();

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
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(GetCache)
                   .As<ICache>()
                   .SingleInstance();

            // РЕАЛИЗАЦИИ ШИНЫ СООБЩЕНИЙ

            builder.RegisterType<MemoryMessageBusManager>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RedisMessageBusManager>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(GetMessageBusManager)
                   .As<IMessageBusManager>()
                   .SingleInstance();

            builder.RegisterType<MemoryMessageBusPublisher>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RedisMessageBusPublisher>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterFactory(GetMessageBusPublisher)
                   .As<IMessageBusPublisher>()
                   .SingleInstance();

            builder.RegisterType<MessageBusImpl>()
                   .As<IMessageBus>()
                   .SingleInstance();

            // СЕССИЯ ПОЛЬЗОВАТЕЛЯ

            builder.RegisterType<SessionManager>()
                   .As<ISessionManager>()
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


        private static ICache GetCache(IContainerResolver resolver)
        {
            var appSettings = resolver.Resolve<IAppEnvironment>();
            var cacheSettings = resolver.Resolve<CacheSettings>();

            ICache cache;

            var keyspace = appSettings.Name;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisCacheFactory = resolver.Resolve<Func<string, RedisCacheImpl>>();

                cache = redisCacheFactory(keyspace);
            }
            else if (string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var memoryCacheFactory = resolver.Resolve<Func<string, MemoryCacheImpl>>();
                var redisCacheFactory = resolver.Resolve<Func<string, RedisCacheImpl>>();
                var twoLayerCacheFactory = resolver.Resolve<TwoLayerCacheFactory>();

                cache = twoLayerCacheFactory(memoryCacheFactory(keyspace), redisCacheFactory(keyspace));
            }
            else
            {
                var memoryCacheFactory = resolver.Resolve<Func<string, MemoryCacheImpl>>();

                cache = memoryCacheFactory(keyspace);
            }

            return cache;
        }

        private static IMessageBusManager GetMessageBusManager(IContainerResolver resolver)
        {
            var appSettings = resolver.Resolve<IAppEnvironment>();
            var cacheSettings = resolver.Resolve<CacheSettings>();

            var keyspace = appSettings.Name;

            IMessageBusManager messageBusManager;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCacheKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisMessageBusManagerFactory = resolver.Resolve<Func<string, RedisMessageBusManager>>();

                messageBusManager = redisMessageBusManagerFactory(keyspace);
            }
            else
            {
                messageBusManager = resolver.Resolve<MemoryMessageBusManager>();
            }

            return messageBusManager;
        }

        private static IMessageBusPublisher GetMessageBusPublisher(IContainerResolver resolver)
        {
            var appSettings = resolver.Resolve<IAppEnvironment>();
            var cacheSettings = resolver.Resolve<CacheSettings>();

            var keyspace = appSettings.Name;

            IMessageBusPublisher messageBusPublisher;

            if (string.Equals(cacheSettings.Type, CacheSettings.RedisCacheKey, StringComparison.OrdinalIgnoreCase)
                || string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
            {
                var redisMessageBusPublisherFactory = resolver.Resolve<Func<string, RedisMessageBusPublisher>>();

                messageBusPublisher = redisMessageBusPublisherFactory(keyspace);
            }
            else
            {
                messageBusPublisher = resolver.Resolve<MemoryMessageBusPublisher>();
            }

            return messageBusPublisher;
        }


        private delegate TwoLayerCacheImpl TwoLayerCacheFactory(ICache memoryCache, ICache sharedCache);
    }
}