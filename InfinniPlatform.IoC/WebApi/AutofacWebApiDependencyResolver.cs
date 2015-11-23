using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;

using Autofac;

namespace InfinniPlatform.IoC.WebApi
{
    internal sealed class AutofacWebApiDependencyResolver : IDependencyResolver
    {
        public AutofacWebApiDependencyResolver(ILifetimeScope container)
        {
            _container = container;
            _rootDependencyScope = new AutofacWebApiDependencyScope(container);
        }


        private readonly ILifetimeScope _container;
        private readonly IDependencyScope _rootDependencyScope;


        public object GetService(Type serviceType)
        {
            return _rootDependencyScope.GetService(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _rootDependencyScope.GetServices(serviceType);
        }


        public IDependencyScope BeginScope()
        {
            var lifetimeScope = _container.BeginLifetimeScope();
            return new AutofacWebApiDependencyScope(lifetimeScope);
        }


        public void Dispose()
        {
            _rootDependencyScope.Dispose();
        }
    }
}