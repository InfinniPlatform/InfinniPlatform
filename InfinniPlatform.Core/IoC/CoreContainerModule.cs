using System.Text;
using InfinniPlatform.Aspects;
using InfinniPlatform.Http;
using InfinniPlatform.Http.Middlewares;
using InfinniPlatform.Logging;
using InfinniPlatform.Security;
using InfinniPlatform.Serialization;
using InfinniPlatform.Session;

using IObjectSerializer = InfinniPlatform.Serialization.IObjectSerializer;

namespace InfinniPlatform.IoC
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

            RegisterLoggingComponents(builder);
            RegisterAspectsComponents(builder);
            RegisterSerializationComponents(builder);
            RegisterSecurityComponents(builder);
            RegisterSessionComponents(builder);
            RegisterHttpComponents(builder);
        }

        private static void RegisterLoggingComponents(IContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(PerformanceLogger<>))
                   .As(typeof(IPerformanceLogger<>))
                   .SingleInstance();

            builder.RegisterType<PerformanceLogger<object>>()
                   .As<IPerformanceLogger>()
                   .SingleInstance();

            builder.RegisterType<PerformanceLoggerFactory>()
                   .As<IPerformanceLoggerFactory>()
                   .SingleInstance();
        }

        private static void RegisterAspectsComponents(IContainerBuilder builder)
        {
            builder.RegisterGeneric(typeof(InternalInterceptor<>))
                   .As(typeof(InternalInterceptor<>))
                   .SingleInstance();

            builder.RegisterType<PerformanceLoggerInterceptor>()
                   .AsSelf()
                   .SingleInstance();
        }

        private static void RegisterSerializationComponents(IContainerBuilder builder)
        {
            builder.RegisterInstance(JsonObjectSerializer.DefaultEncoding)
                   .As<Encoding>()
                   .SingleInstance();

            builder.RegisterType<JsonObjectSerializer>()
                   .As<IObjectSerializer>()
                   .As<IJsonObjectSerializer>()
                   .SingleInstance();
        }

        private static void RegisterSecurityComponents(IContainerBuilder builder)
        {
            builder.RegisterType<UserIdentityProvider>()
                   .As<IUserIdentityProvider>()
                   .SingleInstance();
        }

        private static void RegisterSessionComponents(IContainerBuilder builder)
        {
            builder.RegisterType<TenantScopeProvider>()
                   .As<ITenantScopeProvider>()
                   .SingleInstance();

            builder.RegisterType<TenantProvider>()
                   .As<ITenantProvider>()
                   .SingleInstance();
        }

        private static void RegisterHttpComponents(IContainerBuilder builder)
        {
            // Hosting

            builder.RegisterType<HostAddressParser>()
                   .As<IHostAddressParser>()
                   .SingleInstance();

            // Middlewares

            builder.RegisterType<GlobalHandlingAppLayer>()
                   .As<IDefaultAppLayer>()
                   .SingleInstance();

            builder.RegisterType<ErrorHandlingAppLayer>()
                   .As<IDefaultAppLayer>()
                   .SingleInstance();

            builder.RegisterType<MvcAppLayer>()
                   .As<IDefaultAppLayer>()
                   .SingleInstance();

            // Services

            builder.RegisterType<HttpRequestExcutorFactory>()
                   .AsSelf()
                   .SingleInstance();

            builder.RegisterType<MimeTypeResolver>()
                   .As<IMimeTypeResolver>()
                   .SingleInstance();
        }
    }
}