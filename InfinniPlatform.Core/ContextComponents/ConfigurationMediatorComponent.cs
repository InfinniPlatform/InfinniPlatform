using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Metadata;

namespace InfinniPlatform.ContextComponents
{
	public sealed class ConfigurationMediatorComponent : IConfigurationMediatorComponent
	{
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
		private readonly IConfigurationObjectBuilder _configurationObjectBuilder;

		public ConfigurationMediatorComponent(IConfigurationObjectBuilder configurationObjectBuilder, IMetadataConfigurationProvider metadataConfigurationProvider)
		{
			_metadataConfigurationProvider = metadataConfigurationProvider;
			_configurationObjectBuilder = configurationObjectBuilder;
		}


		public IConfigurationObject GetConfiguration(IConfigRequestProvider configRequestProvider)
		{
			var metadataConfig = _metadataConfigurationProvider
					.GetMetadataConfiguration(configRequestProvider.GetConfiguration());

			return _configurationObjectBuilder.GetConfigurationObject(configRequestProvider.GetConfiguration());

		}

		public IConfigurationObjectBuilder ConfigurationBuilder
		{
			get { return _configurationObjectBuilder; }
		}
	}
}
