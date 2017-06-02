using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

namespace InfinniPlatform.IoC
{
    public sealed class ContainerResolver : IContainerResolver
    {
        public ContainerResolver(IContainerServiceRegistry containerRegistry, IServiceProviderAccessor providerAccessor)
        {
            _containerRegistry = containerRegistry;
            _providerAccessor = providerAccessor;
        }


        private readonly IContainerServiceRegistry _containerRegistry;
        private readonly IServiceProviderAccessor _providerAccessor;

        private IServiceProvider ServiceProvider => _providerAccessor.GetProvider();


        public IEnumerable<Type> Services => _containerRegistry.Services;


        public bool IsRegistered<TService>() where TService : class
        {
            return _containerRegistry.IsRegistered<TService>();
        }

        public bool IsRegistered(Type serviceType)
        {
            return _containerRegistry.IsRegistered(serviceType);
        }


        public bool TryResolve<TService>(out TService serviceInstance) where TService : class
        {
            serviceInstance = ServiceProvider.GetService<TService>();

            return serviceInstance != null;
        }

        public bool TryResolve(Type serviceType, out object serviceInstance)
        {
            serviceInstance = ServiceProvider.GetService(serviceType);

            return serviceInstance != null;
        }


        public TService Resolve<TService>() where TService : class
        {
            return ServiceProvider.GetRequiredService<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return ServiceProvider.GetRequiredService(serviceType);
        }


        public TService ResolveOptional<TService>() where TService : class
        {
            return ServiceProvider.GetService<TService>();
        }

        public object ResolveOptional(Type serviceType)
        {
            return ServiceProvider.GetService(serviceType);
        }
    }
}