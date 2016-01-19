using System;
using System.Collections.Generic;

using InfinniPlatform.Core.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata
{
    internal sealed class ConfigurationMetadataProvider : IConfigurationMetadataProvider
    {
        private readonly Dictionary<string, IConfigurationMetadata> _configurations
            = new Dictionary<string, IConfigurationMetadata>(StringComparer.OrdinalIgnoreCase);

        public void AddConfiguration(IConfigurationMetadata configurationMetadata)
        {
            _configurations[configurationMetadata.Configuration] = configurationMetadata;
        }

        public IEnumerable<string> GetConfigurationNames()
        {
            return _configurations.Keys;
        }

        public IConfigurationMetadata GetConfiguration(string configuration)
        {
            IConfigurationMetadata configurationMetadata;

            return !string.IsNullOrEmpty(configuration)
                   && _configurations.TryGetValue(configuration, out configurationMetadata)
                ? configurationMetadata
                : null;
        }
    }
}