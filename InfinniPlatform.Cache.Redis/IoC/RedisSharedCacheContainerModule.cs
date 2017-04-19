using InfinniPlatform.Cache.Diagnostics;
using InfinniPlatform.Core.Abstractions.Diagnostics;
using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Cache.IoC
{
    public class RedisSharedCacheContainerModule : IContainerModule
    {
        public RedisSharedCacheContainerModule(RedisSharedCacheOptions options)
        {
            _options = options;
        }

        private readonly RedisSharedCacheOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            builder.RegisterType<RedisConnectionFactory>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<RedisSharedCache>()
                   .As<ISharedCache>()
                   .SingleInstance();

            builder.RegisterType<RedisSharedCacheStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }
    }
}