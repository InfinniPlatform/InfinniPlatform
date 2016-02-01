using Autofac;

using InfinniPlatform.IoC.Owin.Middleware;
using InfinniPlatform.Owin.Modules;

using Owin;

namespace InfinniPlatform.IoC.Owin.Modules
{
    internal sealed class AutofacOwinHostingModule : IOwinHostingModule
    {
        public AutofacOwinHostingModule(IContainer container)
        {
            _container = container;
        }


        private readonly IContainer _container;


        public OwinHostingModuleType ModuleType => OwinHostingModuleType.IoC;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            builder.Use(typeof(AutofacRequestLifetimeScopeOwinMiddleware), _container);
        }
    }
}