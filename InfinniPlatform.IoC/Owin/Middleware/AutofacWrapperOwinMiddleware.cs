using System.Threading.Tasks;

using Autofac;
using Autofac.Core;

using Microsoft.Owin;

namespace InfinniPlatform.IoC.Owin.Middleware
{
    internal sealed class AutofacWrapperOwinMiddleware<T> : OwinMiddleware where T : OwinMiddleware
    {
        public AutofacWrapperOwinMiddleware(OwinMiddleware next) : base(next)
        {
        }


        public override Task Invoke(IOwinContext context)
        {
            var lifetimeScope = context.Get<ILifetimeScope>(AutofacOwinConstants.LifetimeScopeKey);
            var realMiddleware = lifetimeScope?.ResolveOptional<T>(TypedParameter.From(Next) as Parameter) ?? Next;
            return realMiddleware.Invoke(context);
        }
    }
}