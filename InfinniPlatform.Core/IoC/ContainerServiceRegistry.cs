using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Autofac.Core;

namespace InfinniPlatform.IoC
{
    public class ContainerServiceRegistry : IContainerServiceRegistry
    {
        private readonly IContainer _rootContainer;

        public ContainerServiceRegistry(IContainer rootContainer)
        {
            _rootContainer = rootContainer;

            Services = rootContainer.ComponentRegistry.Registrations
                                    .SelectMany(r => r.Services)
                                    .OfType<IServiceWithType>()
                                    .Select(s => s.ServiceType)
                                    .Distinct();
        }

        public IEnumerable<Type> Services { get; }

        public bool IsRegistered(Type serviceType)
        {
            return ResolutionExtensions.IsRegistered(_rootContainer, serviceType);
        }

        public bool IsRegistered<TService>() where TService : class
        {
            return ResolutionExtensions.IsRegistered<TService>(_rootContainer);
        }
    }
}