using InfinniPlatform.Core.Abstractions.IoC;

using Microsoft.Extensions.Caching.Memory;

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