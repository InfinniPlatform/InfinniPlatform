using System;
using InfinniPlatform.Cache.Abstractions;
using InfinniPlatform.Cache.Redis;
using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.MessageQueue.Abstractions;

namespace InfinniPlatform.Cache.TwoLayer.IoC
{
    internal sealed class TwoLayerCachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
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
            
            builder.RegisterFactory(GetSharedCache)
                   .As<ISharedCache>()
                   .SingleInstance();
        }

 
        private static ISharedCache GetSharedCache(IContainerResolver resolver)
        {
            var redisCacheFactory = resolver.Resolve<Func<AppOptions, RedisCacheImpl>>();
            ISharedCache cache = redisCacheFactory(resolver.Resolve<AppOptions>());

            return cache;
        }
    }
}