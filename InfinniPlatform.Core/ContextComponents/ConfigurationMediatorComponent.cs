using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.ContextComponents
{
    public sealed class ConfigurationMediatorComponent : IConfigurationMediatorComponent
    {
        private readonly IConfigurationObjectBuilder _configurationObjectBuilder;

        public ConfigurationMediatorComponent(IConfigurationObjectBuilder configurationObjectBuilder)
        {
            _configurationObjectBuilder = configurationObjectBuilder;
        }

        public IConfigurationObject GetConfiguration(string version, string configurationId)
        {
            return _configurationObjectBuilder.GetConfigurationObject(configurationId);
        }

        public IConfigurationObjectBuilder ConfigurationBuilder
        {
            get { return _configurationObjectBuilder; }
        }
    }
}