using InfinniPlatform.Caching.Abstractions;
using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Caching.Memory.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<MemoryCacheImpl>()
                   .As<IMemoryCache>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}