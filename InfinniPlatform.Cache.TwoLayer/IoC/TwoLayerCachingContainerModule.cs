using InfinniPlatform.Cache.Abstractions;
using InfinniPlatform.Core.Abstractions.IoC;
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
        }
    }
}