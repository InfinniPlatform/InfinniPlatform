using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Services;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Owin.IoC
{
    internal sealed class OwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<ErrorHandlingOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<SystemInfoOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<SystemInfoOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<ApplicationOwinHostingModule>()
                   .AsSelf()
                   .SingleInstance();
        }
    }
}