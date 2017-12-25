using InfinniPlatform.Cache.Diagnostics;
using InfinniPlatform.Diagnostics;
using InfinniPlatform.IoC;

namespace InfinniPlatform.Cache.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.Cache" />.
    /// </summary>
    public class RedisSharedCacheContainerModule : IContainerModule
    {
        /// <summary>
        /// Creates new instance of <see cref="RedisSharedCacheContainerModule"/>.
        /// </summary>
        public RedisSharedCacheContainerModule(RedisSharedCacheOptions options)
        {
            _options = options;
        }

        private readonly RedisSharedCacheOptions _options;

        /// <inheritdoc />
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