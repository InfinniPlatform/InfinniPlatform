using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
	/// <summary>
	///  Требуется рефакторинг
	/// </summary>
    public sealed class HelpHandler
    {
		private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;

		public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public HelpHandler(IMetadataConfigurationProvider metadataConfigurationProvider)
		{
			_metadataConfigurationProvider = metadataConfigurationProvider;
		}

		public dynamic GetHelp(string id)
		{
			string config = ConfigRequestProvider.GetConfiguration();
			string metadata = ConfigRequestProvider.GetMetadataIdentifier();
			
			var target = new ApplyResultContext()
			{
                Configuration = config,
                Metadata = metadata,
                Item = id
			};

			var metadataConfig = _metadataConfigurationProvider.GetMetadataConfiguration(ConfigRequestProvider.GetConfiguration());

			metadataConfig.MoveWorkflow(metadata, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "GetHelp"), target);

		    return target.Result;
		}
    }
}
