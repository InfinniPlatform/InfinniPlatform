﻿using System.Text;
using InfinniPlatform.Aspects;
using InfinniPlatform.Http;
using InfinniPlatform.Logging;
using InfinniPlatform.Serialization;
using InfinniPlatform.Session;

using IObjectSerializer = InfinniPlatform.Serialization.IObjectSerializer;

namespace InfinniPlatform.IoC
{
    /// <summary>
    /// Dependency registration module for <see cref="InfinniPlatform" />.
    /// </summary>
    public class CoreContainerModule : IContainerModule
    {
        /// <summary>
        /// Creates new instance of <see cref="CoreContainerModule"/>.
        /// </summary>
        public CoreContainerModule(AppOptions options)
        {
            _options = options;
        }

        private readonly AppOptions _options;

        /// <inheritdoc />
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterInstance(_options).AsSelf();

            RegisterLoggingComponents(builder);
            RegisterAspectsComponents(builder);
            RegisterSerializationComponents(builder);
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
            builder.RegisterType<MimeTypeResolver>()
                   .As<IMimeTypeResolver>()
                   .SingleInstance();
        }
    }
}