using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Resolves services registered in IoC-container
    /// </summary>
    public sealed class ContainerResolver : IContainerResolver
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ContainerResolver" />.
        /// </summary>
        /// <param name="containerRegistry">Registry for services registered in container.</param>
        /// <param name="providerAccessor">Accessors for getting <see cref="IServiceProvider" /> instances in current scope.</param>
        public ContainerResolver(IContainerServiceRegistry containerRegistry, IServiceProviderAccessor providerAccessor)
        {
            _containerRegistry = containerRegistry;
            _providerAccessor = providerAccessor;
        }


        private readonly IContainerServiceRegistry _containerRegistry;
        private readonly IServiceProviderAccessor _providerAccessor;

        private IServiceProvider ServiceProvider => _providerAccessor.GetProvider();


        /// <inheritdoc />
        public IEnumerable<Type> Services => _containerRegistry.Services;


        /// <inheritdoc />
        public bool IsRegistered<TService>() where TService : class
        {
            return _containerRegistry.IsRegistered<TService>();
        }

        /// <inheritdoc />
        public bool IsRegistered(Type serviceType)
        {
            return _containerRegistry.IsRegistered(serviceType);
        }


        /// <inheritdoc />
        public bool TryResolve<TService>(out TService serviceInstance) where TService : class
        {
            serviceInstance = ServiceProvider.GetService<TService>();

            return serviceInstance != null;
        }

        /// <inheritdoc />
        public bool TryResolve(Type serviceType, out object serviceInstance)
        {
            serviceInstance = ServiceProvider.GetService(serviceType);

            return serviceInstance != null;
        }


        /// <inheritdoc />
        public TService Resolve<TService>() where TService : class
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        /// <inheritdoc />
        public object Resolve(Type serviceType)
        {
            return ServiceProvider.GetRequiredService(serviceType);
        }


        /// <inheritdoc />
        public TService ResolveOptional<TService>() where TService : class
        {
            return ServiceProvider.GetService<TService>();
        }

        /// <inheritdoc />
        public object ResolveOptional(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }
    }
}