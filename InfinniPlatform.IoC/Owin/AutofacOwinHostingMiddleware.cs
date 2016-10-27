using Autofac;

using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Sdk.Hosting;

using Owin;

namespace InfinniPlatform.IoC.Owin
{
    internal sealed class AutofacOwinHostingMiddleware : OwinHostingMiddleware
    {
        public AutofacOwinHostingMiddleware(IContainer container) : base(HostingMiddlewareType.GlobalHandling)
        {
            _container = container;
        }


        private readonly IContainer _container;


        public override void Configure(IAppBuilder builder)
        {
            builder.Use(typeof(AutofacRequestLifetimeScopeOwinMiddleware), _container);
        }
    }
}