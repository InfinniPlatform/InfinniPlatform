using System;

using Autofac;

using InfinniPlatform.IoC.Owin.Middleware;
using InfinniPlatform.Owin.Modules;

using Owin;

namespace InfinniPlatform.IoC.Owin.Modules
{
    internal sealed class AutofacOwinHostingModule : IOwinHostingModule
    {
        public OwinHostingModuleType ModuleType => OwinHostingModuleType.IoC;


        public void Configure(IAppBuilder builder, IOwinHostingContext context)
        {
            var container = context.ContainerResolver.Resolve<Func<IContainer>>()();

            builder.Use(typeof(AutofacRequestLifetimeScopeOwinMiddleware), container);
        }
    }
}