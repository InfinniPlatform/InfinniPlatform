using InfinniPlatform.IoC;

namespace InfinniPlatform.Cache.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.Cache" />.
    /// </summary>
    public class InMemoryCacheContainerModule : IContainerModule
    {
        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<InMemoryCache>().As<IInMemoryCache>().SingleInstance();
        }
    }
}