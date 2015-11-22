using InfinniPlatform.Metadata.Implementation.MetadataConfiguration;
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
        }
    }
}