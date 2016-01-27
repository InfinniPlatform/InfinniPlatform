using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Security;
using InfinniPlatform.Owin.Services;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Security;
using InfinniPlatform.Sdk.Services;

using Nancy;
using Nancy.Bootstrapper;

namespace InfinniPlatform.Owin.IoC
{
    internal sealed class OwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Модуль Nancy

            builder.RegisterType<NancyOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceNancyBootstrapper>()
                   .As<INancyBootstrapper>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceNancyModuleCatalog>()
                   .As<INancyModuleCatalog>()
                   .SingleInstance();

            builder.RegisterGeneric(typeof(HttpServiceNancyModule<>))
                   .As(typeof(HttpServiceNancyModule<>))
                   .InstancePerDependency();

            builder.RegisterType<HttpServiceNancyModuleInitializer>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<NancyMimeTypeResolver>()
                   .As<IMimeTypeResolver>()
                   .SingleInstance();

            builder.RegisterType<OwinUserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();

            builder.RegisterType<OwinContextProvider>()
                   .As<IOwinContextProvider>()
                   .SingleInstance();

            builder.RegisterType<HttpRequestExcutorFactory>()
                   .AsSelf()
                   .SingleInstance();

            // Остальные модули

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
        }
    }
}