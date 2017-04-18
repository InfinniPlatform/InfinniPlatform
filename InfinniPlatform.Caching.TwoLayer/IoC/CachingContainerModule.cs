﻿using System;
using InfinniPlatform.Caching.Abstractions;
using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.MessageQueue.Abstractions;

namespace InfinniPlatform.Caching.TwoLayer.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
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

            var redisCacheFactory = resolver.Resolve<Func<IAppEnvironment, RedisCacheImpl>>();
            ISharedCache cache = redisCacheFactory(resolver.Resolve<IAppEnvironment>());

            return cache;
        }
    }
}