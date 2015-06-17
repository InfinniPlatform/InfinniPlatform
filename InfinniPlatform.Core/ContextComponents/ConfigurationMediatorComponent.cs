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
