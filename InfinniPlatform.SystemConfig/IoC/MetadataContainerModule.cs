using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.Metadata;
using InfinniPlatform.SystemConfig.RequestHandlers;

namespace InfinniPlatform.SystemConfig.IoC
{
    internal sealed class MetadataContainerModule : IContainerModule
    {
        public void Load(IContainerBuilder builder)
        {
            builder.RegisterType<MetadataComponent>()
                   .As<IMetadataComponent>()
                   .SingleInstance();

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

            builder.RegisterType<RequestHandlerInstaller>()
                   .As<IRequestHandlerInstaller>()
                   .SingleInstance();

            // Обработчики HTTP-запросов

            builder.RegisterType<ApplyChangesHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<SearchHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<UploadHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<UrlEncodedDataHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            builder.RegisterType<SearchDocumentAggregationHandler>()
                   .As<IWebRoutingHandler>()
                   .AsSelf();

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}