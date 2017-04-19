using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

namespace InfinniPlatform.Core.IoC
{
    public sealed class AutofacContainerResolver : IContainerResolver
    {
        public AutofacContainerResolver(IComponentContext rootContainer) : this(rootContainer, () => rootContainer)
        {
        }


        public AutofacContainerResolver(IComponentContext rootContainer, Func<IComponentContext> requestContainerProvider)
        {
            _rootContainer = rootContainer;
            _requestContainerProvider = requestContainerProvider;
        }


        private readonly IComponentContext _rootContainer;
        private readonly Func<IComponentContext> _requestContainerProvider;


        public IEnumerable<Type> Services => _rootContainer.ComponentRegistry.Registrations
                                                           .SelectMany(r => r.Services)
                                                           .OfType<IServiceWithType>()
                                                           .Select(s => s.ServiceType)
                                                           .Distinct();


        private IComponentContext Container => _requestContainerProvider() ?? _rootContainer;


        public bool IsRegistered<TService>() where TService : class
        {
            return Container.IsRegistered<TService>();
        }

        public bool IsRegistered(Type serviceType)
        {
            return Container.IsRegistered(serviceType);
        }

        public bool TryResolve<TService>(out TService serviceInstance) where TService : class
        {
            return Container.TryResolve(out serviceInstance);
        }

        public bool TryResolve(Type serviceType, out object serviceInstance)
        {
            return Container.TryResolve(serviceType, out serviceInstance);
        }

        public TService Resolve<TService>() where TService : class
        {
            return Container.Resolve<TService>();
        }

        public object Resolve(Type serviceType)
        {
            return Container.Resolve(serviceType);
        }

        public TService ResolveOptional<TService>() where TService : class
        {
            return Container.ResolveOptional<TService>();
        }

        public object ResolveOptional(Type serviceType)
        {
            return Container.ResolveOptional(serviceType);
        }
    }
}