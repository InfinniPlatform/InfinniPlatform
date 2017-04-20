using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Cache.IoC
{
    internal sealed class TwoLayerCacheContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<TwoLayerCache>()
                   .As<ITwoLayerCache>()
                   .As<ITwoLayerCacheSynchronizer>()
                   .SingleInstance();

            builder.RegisterType<TwoLayerCacheConsumer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<TwoLayerCacheConsumerSource>()
                   .As<IMessageConsumerSource>()
                   .SingleInstance();
        }
    }
}