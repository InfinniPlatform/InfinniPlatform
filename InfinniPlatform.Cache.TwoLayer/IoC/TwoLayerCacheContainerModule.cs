using InfinniPlatform.Cache.Clusterization;
using InfinniPlatform.IoC;
using InfinniPlatform.MessageQueue;

namespace InfinniPlatform.Cache.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform.Cache" />.
    /// </summary>
    public class TwoLayerCacheContainerModule : IContainerModule
    {
        /// <summary>
        /// Creates new instance of <see cref="TwoLayerCacheContainerModule"/>.
        /// </summary>
        public TwoLayerCacheContainerModule(TwoLayerCacheOptions options)
        {
            _options = options;
        }
        
        private readonly TwoLayerCacheOptions _options;

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf().SingleInstance();

            builder.RegisterType<InMemoryCacheFactory>().AsSelf().SingleInstance();
            builder.RegisterFactory(CreateInMemoryCacheFactory).As<IInMemoryCacheFactory>().SingleInstance();

            builder.RegisterType<SharedCacheFactory>().AsSelf().SingleInstance();
            builder.RegisterFactory(CreateSharedCacheFactory).As<ISharedCacheFactory>().SingleInstance();

            builder.RegisterType<TwoLayerCacheStateObserver>().AsSelf().SingleInstance();
            builder.RegisterType<TwoLayerCacheStateObserverStub>().AsSelf().SingleInstance();
            builder.RegisterFactory(CreateTwoLayerCacheStateObserver).As<ITwoLayerCacheStateObserver>().SingleInstance();

            builder.RegisterType<TwoLayerCache>().As<ITwoLayerCache>().SingleInstance();

            // Clusterization

            builder.RegisterType<TwoLayerCacheResetKeyConsumer>().As<IConsumer>().SingleInstance();
        }


        private IInMemoryCacheFactory CreateInMemoryCacheFactory(IContainerResolver resolver)
        {
            return _options.InMemoryCacheFactory?.Invoke(resolver) ?? resolver.Resolve<InMemoryCacheFactory>();
        }

        private ISharedCacheFactory CreateSharedCacheFactory(IContainerResolver resolver)
        {
            return _options.SharedCacheFactory?.Invoke(resolver) ?? resolver.Resolve<SharedCacheFactory>();
        }

        private ITwoLayerCacheStateObserver CreateTwoLayerCacheStateObserver(IContainerResolver resolver)
        {
            var twoLayerCacheStateObserver = _options.TwoLayerCacheStateObserver?.Invoke(resolver);

            if (twoLayerCacheStateObserver == null)
            {
                if (TwoLayerCacheStateObserver.CanBeCreated(resolver))
                {
                    twoLayerCacheStateObserver = resolver.Resolve<TwoLayerCacheStateObserver>();
                }
                else
                {
                    twoLayerCacheStateObserver = resolver.Resolve<TwoLayerCacheStateObserverStub>();
                }
            }

            return twoLayerCacheStateObserver;
        }
    }
}