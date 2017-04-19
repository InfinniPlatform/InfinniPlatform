using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Cache.IoC
{
    public class InMemoryCacheContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<InMemoryCacheImpl>().As<IInMemoryCache>().SingleInstance();
        }
    }
}