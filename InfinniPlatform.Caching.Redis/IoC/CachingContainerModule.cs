using InfinniPlatform.Caching.Redis;
using InfinniPlatform.Caching.Redis.Diagnostics;
using InfinniPlatform.Core.Abstractions.Diagnostics;
using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Settings;

namespace InfinniPlatform.Caching.IoC
{
    internal sealed class CachingContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterFactory(GetRedisConnectionSettings)
                   .As<RedisConnectionSettings>()
                   .SingleInstance();

            builder.RegisterType<RedisConnectionFactory>()
                   .AsSelf()
                   .InstancePerDependency();

            builder.RegisterType<RedisCacheImpl>()
                   .AsSelf()
                   .SingleInstance();

            // Diagnostic

            builder.RegisterType<CachingStatusProvider>()
                   .As<ISubsystemStatusProvider>()
                   .SingleInstance();
        }


        private static RedisConnectionSettings GetRedisConnectionSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>()
                           .GetSection<RedisConnectionSettings>(RedisConnectionSettings.SectionName);
        }
    }
}