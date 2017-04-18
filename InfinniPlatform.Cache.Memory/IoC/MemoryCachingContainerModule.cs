using InfinniPlatform.Cache.Abstractions;
using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Cache.Memory.IoC
{
    internal sealed class MemoryCachingContainerModule : IContainerModule
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