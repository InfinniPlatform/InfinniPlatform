using InfinniPlatform.Core.ContextComponents;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Runtime;
using InfinniPlatform.Sdk.IoC;
using InfinniPlatform.SystemConfig.Metadata;

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

            // Прикладные скрипты
            builder.RegisterActionUnits(GetType().Assembly);
        }
    }
}