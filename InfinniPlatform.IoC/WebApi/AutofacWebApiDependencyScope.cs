using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using Autofac;

namespace InfinniPlatform.IoC.WebApi
{
    internal sealed class AutofacWebApiDependencyScope : IDependencyScope
    {
        public AutofacWebApiDependencyScope(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }


        private readonly ILifetimeScope _lifetimeScope;


        public object GetService(Type serviceType)
        {
            return _lifetimeScope.ResolveOptional(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!_lifetimeScope.IsRegistered(serviceType))
            {
                return Enumerable.Empty<object>();
            }

            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = _lifetimeScope.Resolve(enumerableServiceType);

            return (IEnumerable<object>)instance;
        }


        public void Dispose()
        {
            _lifetimeScope?.Dispose();
        }
    }
}