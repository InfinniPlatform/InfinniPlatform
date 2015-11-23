using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Implementation.Handlers;
using InfinniPlatform.Metadata.Implementation.MetadataConfiguration;
using InfinniPlatform.Metadata.Implementation.Modules;
using InfinniPlatform.Modules;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.Metadata.IoC
{
    internal sealed class MetadataContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<MetadataConfigurationProvider>()
                   .As<IMetadataConfigurationProvider>()
                   .SingleInstance();

            builder.RegisterType<ConfigurationObjectBuilder>()
                   .As<IConfigurationObjectBuilder>()
                   .SingleInstance();

            builder.RegisterType<ScriptConfiguration>()
                   .As<IScriptConfiguration>()
                   .SingleInstance();

            builder.RegisterType<CrossConfigSearcher>()
                   .As<ICrossConfigSearcher>()
                   .SingleInstance();

            builder.RegisterType<StandardTemplatesInstaller>()
                   .As<ITemplateInstaller>()
                   .SingleInstance();

            // Обработчики HTTP-запросов

            builder.RegisterType<ApplyChangesHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<SearchHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<SystemEventsHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<UploadHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<UrlEncodedDataHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<CustomServiceHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<SearchDocumentAggregationHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();
        }
    }
}