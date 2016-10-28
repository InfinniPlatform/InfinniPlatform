using InfinniPlatform.Owin.Hosting;
using InfinniPlatform.Owin.Middleware;
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
            // Settings

            builder.RegisterFactory(GetHostingConfig)
                   .As<HostingConfig>()
                   .SingleInstance();

            builder.RegisterFactory(GetStaticContentSettings)
                   .As<StaticContentSettings>()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<HostAddressParser>()
                   .As<IHostAddressParser>()
                   .SingleInstance();

            builder.RegisterType<OwinHostingService>()
                   .As<IHostingService>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<ErrorHandlingOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            // Nancy

            builder.RegisterType<NancyOwinHostingMiddleware>()
                   .As<IHostingMiddleware>()
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
        }


        private static HostingConfig GetHostingConfig(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<HostingConfig>(HostingConfig.SectionName);
        }

        private static StaticContentSettings GetStaticContentSettings(IContainerResolver resolver)
        {
            return resolver.Resolve<IAppConfiguration>().GetSection<StaticContentSettings>(StaticContentSettings.SectionName);
        }

    }
}