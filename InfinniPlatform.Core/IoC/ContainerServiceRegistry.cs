using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

namespace InfinniPlatform.IoC
{
    public class ContainerServiceRegistry : IContainerServiceRegistry
    {
        public ContainerServiceRegistry(IContainer rootContainer)
        {
            _rootContainer = rootContainer;

            Services = rootContainer.ComponentRegistry.Registrations
                                    .SelectMany(r => r.Services)
                                    .OfType<IServiceWithType>()
                                    .Select(s => s.ServiceType)
                                    .Distinct();
        }


        private readonly IContainer _rootContainer;


        public IEnumerable<Type> Services { get; }


        public bool IsRegistered(Type serviceType)
        {
            return _rootContainer.IsRegistered(serviceType);
        }

        public bool IsRegistered<TService>() where TService : class
        {
            return _rootContainer.IsRegistered<TService>();
        }
    }
}