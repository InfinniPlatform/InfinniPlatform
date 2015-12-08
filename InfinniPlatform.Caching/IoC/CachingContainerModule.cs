using InfinniPlatform.Caching.Factory;
using InfinniPlatform.Caching.Session;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<CacheMessageBusFactory>()
                   .As<ICacheMessageBusFactory>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<ICacheMessageBusFactory>().CreateCacheMessageBus())
                   .As<ICacheMessageBus>()
                   .SingleInstance();

            builder.RegisterType<CacheFactory>()
                   .As<ICacheFactory>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<ICacheFactory>().CreateCache())
                   .As<ICache>()
                   .SingleInstance();

            builder.RegisterType<SessionManager>()
                   .As<ISessionManager>()
                   .SingleInstance();
        }
    }
}