using System.Web.Http.Dependencies;

using Autofac;

namespace InfinniPlatform.IoC.WebApi
{
    internal sealed class AutofacWebApiDependencyResolver : AutofacWebApiDependencyScope, IDependencyResolver
    {
        public AutofacWebApiDependencyResolver(ILifetimeScope lifetimeScope) : base(lifetimeScope)
        {
        }


        public IDependencyScope BeginScope()
        {
            var lifetimeScope = LifetimeScope.BeginLifetimeScope();

            return new AutofacWebApiDependencyScope(lifetimeScope);
        }
    }
}