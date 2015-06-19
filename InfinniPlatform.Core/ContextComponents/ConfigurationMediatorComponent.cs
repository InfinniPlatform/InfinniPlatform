using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Metadata;

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
            return _configurationObjectBuilder.GetConfigurationObject(version, configurationId);
        }

        public IConfigurationObjectBuilder ConfigurationBuilder
        {
            get { return _configurationObjectBuilder; }
        }
    }
}