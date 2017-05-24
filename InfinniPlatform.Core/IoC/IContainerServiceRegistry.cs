using System;
using System.Collections.Generic;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Registry for services registered in container.
    /// </summary>
    public interface IContainerServiceRegistry
    {
        /// <summary>
        /// List of registered services.
        /// </summary>
        IEnumerable<Type> Services { get; }

        /// <summary>
        /// Checks if service is registered in container.
        /// </summary>
        /// <param name="serviceType">Service type.</param>
        bool IsRegistered(Type serviceType);

        /// <summary>
        /// Checks if service is registered in container
        /// </summary>
        /// <typeparam name="TService">Service type.</typeparam>
        bool IsRegistered<TService>() where TService : class;
    }
}