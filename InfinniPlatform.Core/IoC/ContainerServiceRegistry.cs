using System;
using System.Collections.Generic;
using System.Linq;

using Autofac;
using Autofac.Core;

namespace InfinniPlatform.IoC
{
    /// <inheritdoc />
    public class ContainerServiceRegistry : IContainerServiceRegistry
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ContainerServiceRegistry" />.
        /// </summary>
        /// <param name="rootContainer">Autofac root container.</param>
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


        /// <inheritdoc />
        public IEnumerable<Type> Services { get; }


        /// <inheritdoc />
        public bool IsRegistered(Type serviceType)
        {
            return _rootContainer.IsRegistered(serviceType);
        }

        /// <inheritdoc />
        public bool IsRegistered<TService>() where TService : class
        {
            return _rootContainer.IsRegistered<TService>();
        }
    }
}