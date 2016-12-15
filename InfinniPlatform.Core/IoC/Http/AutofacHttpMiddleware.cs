using Autofac;

using InfinniPlatform.Http.Middlewares;

using Owin;

namespace InfinniPlatform.Core.IoC.Http
{
    internal sealed class AutofacHttpMiddleware : HttpMiddleware
    {
        public AutofacHttpMiddleware(IContainer container) : base(HttpMiddlewareType.GlobalHandling)
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