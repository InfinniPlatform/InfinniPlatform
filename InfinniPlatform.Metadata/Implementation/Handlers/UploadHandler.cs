using System.IO;
using InfinniPlatform.Api.Context;
using InfinniPlatform.Api.ContextComponents;
using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Metadata.Implementation.Handlers
{
	public sealed class UploadHandler : IWebRoutingHandler 
	{
		private readonly IGlobalContext _globalContext;

		public IConfigRequestProvider ConfigRequestProvider { get; set; }

		public UploadHandler(IGlobalContext globalContext)
		{
			
			_globalContext = globalContext;
		}

		public dynamic UploadFile(dynamic linkedData, Stream uploadStream)
		{
			string config = ConfigRequestProvider.GetConfiguration();
			string metadata = ConfigRequestProvider.GetMetadataIdentifier();
		    string version = ConfigRequestProvider.GetVersion();
			
			var target = new UploadContext
			{
				IsValid = true,
				LinkedData = linkedData,
				Configuration = config,
				Metadata = metadata,
				FileContent = uploadStream,
				Context = _globalContext
			};

            var metadataConfig = _globalContext.GetComponent<IMetadataConfigurationProvider>(ConfigRequestProvider.GetVersion()).GetMetadataConfiguration(version, ConfigRequestProvider.GetConfiguration());

			metadataConfig.MoveWorkflow(metadata, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "Upload"), target);

			if (target.IsValid)
			{
				dynamic item = new DynamicWrapper();
				item.IsValid = true;
				item.ValidationMessage = Resources.DocumentContentSuccessfullyUploaded;
				item.Result = target.Result;
				return AggregateExtensions.PrepareResultAggregate(item);
			}
			return AggregateExtensions.PrepareInvalidResultAggregate(target);
		}
	}
}
