using System.Threading.Tasks;

using Autofac;
using Autofac.Core.Lifetime;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Owin.Middleware
{
    internal sealed class AutofacRequestLifetimeScopeOwinMiddleware : OwinMiddleware
    {
        public AutofacRequestLifetimeScopeOwinMiddleware(OwinMiddleware next, ILifetimeScope container) : base(next)
        {
            _container = container;
        }


        private readonly ILifetimeScope _container;


        public override Task Invoke(IOwinContext context)
        {
            using (var lifetimeScope = _container.BeginLifetimeScope(MatchingScopeLifetimeTags.RequestLifetimeScopeTag, b => b.RegisterInstance(context).As<IOwinContext>()))
            {
                context.Set(AutofacOwinConstants.LifetimeScopeKey, lifetimeScope);

                return Next.Invoke(context);
            }
        }
    }
}