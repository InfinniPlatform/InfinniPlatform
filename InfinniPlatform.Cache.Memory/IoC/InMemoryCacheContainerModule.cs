using InfinniPlatform.IoC;

namespace InfinniPlatform.Cache.IoC
{
    public class InMemoryCacheContainerModule : IContainerModule
    {
        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<InMemoryCache>().As<IInMemoryCache>().SingleInstance();
        }
    }
}