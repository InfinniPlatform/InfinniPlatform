using System;

using InfinniPlatform.Caching.Memory;
using InfinniPlatform.Caching.RabbitMQ;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.Session;
using InfinniPlatform.Caching.TwoLayer;
using InfinniPlatform.Sdk.Cache;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Queues;
using InfinniPlatform.Sdk.Queues.Consumers;
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
                   .As<IBroadcastConsumer>()
                   .As<ICache>()
                   .SingleInstance();

            builder.RegisterType<MemoryCacheImpl>()
                   .As<IMemoryCache>()
                   .SingleInstance();

            builder.RegisterFactory(GetSharedCache)
                   .As<ISharedCache>()
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

        private static ISharedCache GetSharedCache(IContainerResolver resolver)
        {
            var cacheSettings = resolver.Resolve<CacheSettings>();

            if (cacheSettings.Type == CacheSettings.MemoryCacheKey)
            {
                return new RedisCacheStubImpl();
            }

            var redisCacheFactory = resolver.Resolve<Func<IAppEnvironment, RedisCacheImpl>>();
            ISharedCache cache = redisCacheFactory(resolver.Resolve<IAppEnvironment>());

            return cache;
        }

        private static IMessageBusManager GetMessageBusManager(IContainerResolver resolver)
        {
            var appSettings = resolver.Resolve<IAppEnvironment>();
            var cacheSettings = resolver.Resolve<CacheSettings>();

            var keyspace = appSettings.Name;

            IMessageBusManager messageBusManager;

            if (string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
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

            if (string.Equals(cacheSettings.Type, CacheSettings.TwoLayerCacheKey, StringComparison.OrdinalIgnoreCase))
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
    }
}