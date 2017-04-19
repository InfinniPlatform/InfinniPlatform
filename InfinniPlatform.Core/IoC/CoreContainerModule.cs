using System.Text;

using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Http;
using InfinniPlatform.Core.Http.Middlewares;
using InfinniPlatform.Core.Http.Services;
using InfinniPlatform.Core.IoC.Http;
using InfinniPlatform.Core.Security;
using InfinniPlatform.Core.Serialization;
using InfinniPlatform.Core.Session;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Http.Middlewares;

namespace InfinniPlatform.Core.IoC
{
    public class CoreContainerModule : IContainerModule
    {
        public CoreContainerModule(AppOptions options)
        {
            _options = options;
        }

        private readonly AppOptions _options;

        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            RegisterIoCComponents(builder);
            RegisterDiagnosticsComponents(builder);
            RegisterSerializationComponents(builder);
            RegisterSecurityComponents(builder);
            RegisterSessionComponents(builder);
            RegisterHttpComponents(builder);
        }


        private void RegisterIoCComponents(IContainerBuilder builder)
        {
            builder.RegisterType<AutofacOwinMiddlewareResolver>()
                   .As<IOwinMiddlewareTypeResolver>()
                   .SingleInstance();

            builder.RegisterType<AutofacRequestLifetimeScopeOwinMiddleware>()
                   .As<OwinMiddleware>()
                   .SingleInstance();
        }

        private void RegisterDiagnosticsComponents(IContainerBuilder builder)
        {
            builder.RegisterType<SystemInfoHttpService>()
                   .As<IHttpService>()
                   .SingleInstance();
        }

        private void RegisterSerializationComponents(IContainerBuilder builder)
        {
            builder.RegisterInstance(JsonObjectSerializer.DefaultEncoding)
                   .As<Encoding>()
                   .SingleInstance();

            builder.RegisterType<JsonObjectSerializer>()
                   .As<IObjectSerializer>()
                   .As<IJsonObjectSerializer>()
                   .SingleInstance();
        }

        private void RegisterSecurityComponents(IContainerBuilder builder)
        {
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();
        }

        private void RegisterSessionComponents(IContainerBuilder builder)
        {
            builder.RegisterType<TenantScopeProvider>()
                   .As<ITenantScopeProvider>()
                   .SingleInstance();

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();
        }

        private void RegisterHttpComponents(IContainerBuilder builder)
        {
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
                   .As<Nancy.Bootstrapper.INancyBootstrapper>()
                   .SingleInstance();

            builder.RegisterType<HttpServiceNancyModuleCatalog>()
                   .As<Nancy.INancyModuleCatalog>()
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
    }
}