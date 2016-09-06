using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Middleware;
using InfinniPlatform.Owin.Modules;
using InfinniPlatform.Owin.Security;
using InfinniPlatform.Owin.Services;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Services;
using InfinniPlatform.Sdk.Settings;

using Nancy;
using Nancy.Bootstrapper;

namespace InfinniPlatform.Owin.IoC
{
    internal sealed class OwinContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Nancy

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

            builder.RegisterType<OwinContextProvider>()
                   .As<IOwinContextProvider>()
                   .SingleInstance();

            builder.RegisterType<HttpRequestExcutorFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<HttpServiceSource>()
                   .As<IHttpServiceSource>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceContext>()
                   .As<IHttpServiceContext>()
                   .InstancePerRequest();

            builder.RegisterType<HttpServiceContextProvider>()
                   .As<IHttpServiceContextProvider>()
                   .SingleInstance();

            // ErrorHandling

            builder.RegisterType<ErrorHandlingOwinHostingModule>()
                   .As<IOwinHostingModule>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            // StaticContent

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<StaticContentSettings>(StaticContentSettings.SectionName))
                   .As<StaticContentSettings>()
                   .SingleInstance();

            // Hosting

            builder.RegisterInstance(HostAddressParser.Default)
                   .As<IHostAddressParser>()
                   .SingleInstance();
        }
    }
}