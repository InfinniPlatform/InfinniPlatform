using InfinniPlatform.Core.Compression;
using InfinniPlatform.Core.Diagnostics;
using InfinniPlatform.Core.Logging;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Settings;
using InfinniPlatform.Core.StartupInitializers;
using InfinniPlatform.Core.Transactions;
using InfinniPlatform.Sdk.Hosting;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.Sdk.Logging;
using InfinniPlatform.Sdk.Serialization;
using InfinniPlatform.Sdk.Session;
using InfinniPlatform.Sdk.Settings;

namespace InfinniPlatform.Core.IoC
{
    internal class CoreContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            // Diagnostics

            builder.RegisterType<SystemStatusProvider>()
                   .As<ISystemStatusProvider>()
                   .SingleInstance();

            // Configuration

            builder.RegisterInstance(AppConfiguration.Instance)
                   .As<IAppConfiguration>();

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<AppEnvironment>(AppEnvironment.SectionName))
                   .As<IAppEnvironment>()
                   .SingleInstance();

            builder.RegisterType<GZipDataCompressor>()
                   .As<IDataCompressor>()
                   .SingleInstance();

            // Logging

            builder.OnCreateInstance(new LogContainerParameterResolver<ILog>(LogManagerCache.GetLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<ILog>(LogManagerCache.GetLog));

            builder.OnCreateInstance(new LogContainerParameterResolver<IPerformanceLog>(LogManagerCache.GetPerformanceLog));
            builder.OnActivateInstance(new LogContainerInstanceActivator<IPerformanceLog>(LogManagerCache.GetPerformanceLog));

            // Transaction

            builder.RegisterType<DocumentTransactionScopeProvider>()
                   .As<IDocumentTransactionScopeProvider>()
                   .SingleInstance();

            // Serialization

            builder.RegisterType<JsonObjectSerializer>()
                   .As<IObjectSerializer>()
                   .As<IJsonObjectSerializer>()
                   .SingleInstance();

            // Metadata

            builder.RegisterFactory(r => r.Resolve<IAppConfiguration>().GetSection<MetadataSettings>(MetadataSettings.SectionName))
                   .As<MetadataSettings>()
                   .SingleInstance();

            builder.RegisterType<MetadataApi>()
                   .As<IMetadataApi>()
                   .AsSelf()
                   .SingleInstance();

            // Hosting

            builder.RegisterType<PackageJsonConfigurationsInitializer>()
                   .As<IApplicationEventHandler>()
                   .SingleInstance();
        }
    }
}