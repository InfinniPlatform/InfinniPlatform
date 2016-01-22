using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Dependencies;

using Autofac;

namespace InfinniPlatform.IoC.WebApi
{
    internal class AutofacWebApiDependencyScope : IDependencyScope
    {
        public AutofacWebApiDependencyScope(ILifetimeScope lifetimeScope)
        {
            LifetimeScope = lifetimeScope;
        }


        protected readonly ILifetimeScope LifetimeScope;


        public object GetService(Type serviceType)
        {
            return LifetimeScope.ResolveOptional(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            if (!LifetimeScope.IsRegistered(serviceType))
            {
                return Enumerable.Empty<object>();
            }

            var enumerableServiceType = typeof(IEnumerable<>).MakeGenericType(serviceType);
            var instance = LifetimeScope.Resolve(enumerableServiceType);

            return (IEnumerable<object>)instance;
        }


        public void Dispose()
        {
            if (LifetimeScope != null)
            {
                LifetimeScope.Dispose();
            }
        }
    }
}