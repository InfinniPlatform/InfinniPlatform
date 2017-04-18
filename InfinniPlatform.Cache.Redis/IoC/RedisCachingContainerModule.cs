using InfinniPlatform.Cache.Abstractions;
using InfinniPlatform.Cache.Redis.Diagnostics;
using InfinniPlatform.Core.Abstractions.Diagnostics;
using InfinniPlatform.Core.Abstractions.IoC;

namespace InfinniPlatform.Cache.Redis.IoC
{
    internal sealed class RedisCachingContainerModule : IContainerModule
    {
        private readonly RedisCacheOptions _redisOptions;

        public RedisCachingContainerModule(RedisCacheOptions redisOptions)
        {
            _redisOptions = redisOptions;
        }

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_redisOptions)
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<RedisConnectionFactory>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<RedisCacheImpl>()
                   .AsSelf()
                   .As<ISharedCache>()
                   .SingleInstance();

            builder.RegisterType<CachingStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }
    }
}