using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.IoC
{
    internal sealed class AutofacContainerResolver : IContainerResolver
    {
        public AutofacContainerResolver(IComponentContext componentContext)
        {
            _componentContext = componentContext;
        }


        private readonly IComponentContext _componentContext;


        public IEnumerable<Type> Services => _componentContext.ComponentRegistry.Registrations
                                                              .SelectMany(r => r.Services)
                                                              .OfType<IServiceWithType>()
                                                              .Select(s => s.ServiceType)
                                                              .Distinct();


        public bool IsRegistered<TService>() where TService : class
        {
            return _componentContext.IsRegistered<TService>();
        }

        public bool IsRegistered(Type serviceType)
        {
            return _componentContext.IsRegistered(serviceType);
        }

        public bool TryResolve<TService>(out TService serviceInstance) where TService : class
        {
            return _componentContext.TryResolve(out serviceInstance);
        }

        public bool TryResolve(Type serviceType, out object serviceInstance)
        {
            return _componentContext.TryResolve(serviceType, out serviceInstance);
        }

        public TService Resolve<TService>() where TService : class
        {
            return _componentContext.Resolve<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return _componentContext.Resolve(serviceType);
        }

        public TService ResolveOptional<TService>() where TService : class
        {
            return _componentContext.ResolveOptional<TService>();
        }

        public object ResolveOptional(Type serviceType)
        {
            return _componentContext.ResolveOptional(serviceType);
        }
    }
}