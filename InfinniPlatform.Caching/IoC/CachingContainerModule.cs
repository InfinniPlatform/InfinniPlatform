using InfinniPlatform.Caching.Factory;
using InfinniPlatform.Factories;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<SessionManagerFactory>()
                   .As<ISessionManagerFactory>()
                   .SingleInstance();

            builder.RegisterFactory(r => r.Resolve<ISessionManagerFactory>().CreateSessionManager())
                   .As<ISessionManager>()
                   .SingleInstance();
        }
    }
}