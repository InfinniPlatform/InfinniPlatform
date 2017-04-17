using InfinniPlatform.Core.Abstractions.Http;
using InfinniPlatform.Core.Abstractions.IoC;
using InfinniPlatform.Core.Abstractions.Settings;
using InfinniPlatform.Core.Http.Middlewares;
using InfinniPlatform.Core.Http.Services;
using InfinniPlatform.Http.Middlewares;

using Nancy;
using Nancy.Bootstrapper;

namespace InfinniPlatform.Core.Http.IoC
{
    internal class HttpContainerModule : IContainerModule
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

            // Middlewares

            builder.RegisterType<HttpContextProvider>()
                   .As<IHttpContextProvider>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingOwinMiddleware>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<NancyHttpMiddleware>()
                   .As<IHttpMiddleware>()
                   .SingleInstance();

            // Services

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

            builder.RegisterType<HttpRequestExcutorFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<HttpServiceSource>()
                   .As<IHttpServiceSource>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceContext>()
                   .As<IHttpServiceContext>()
                   .InstancePerDependency();

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