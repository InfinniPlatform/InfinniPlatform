using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.Core.ContextComponents
{
    public sealed class ConfigurationMediatorComponent : IConfigurationMediatorComponent
    {
        public ConfigurationMediatorComponent(IConfigurationObjectBuilder configurationObjectBuilder)
        {
            ConfigurationBuilder = configurationObjectBuilder;
        }

        public IConfigurationObject GetConfiguration(string configurationId)
        {
            return ConfigurationBuilder.GetConfigurationObject(configurationId);
        }

        public IConfigurationObjectBuilder ConfigurationBuilder { get; }
    }
}