using Autofac;

using InfinniPlatform.IoC.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Sdk.Logging;

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


        public void Configure(IAppBuilder builder, IOwinHostingContext context, ILog log)
        {
            builder.Use(typeof(AutofacRequestLifetimeScopeOwinMiddleware), _container);
        }
    }
}