using System.IO;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.Metadata.Handlers
{
    public sealed class UploadHandler : IWebRoutingHandler
    {
        private readonly IGlobalContext _globalContext;

        public UploadHandler(IGlobalContext globalContext)
        {
            _globalContext = globalContext;
        }

        public IConfigRequestProvider ConfigRequestProvider { get; set; }

        public dynamic UploadFile(dynamic linkedData, Stream uploadStream)
        {
            var config = ConfigRequestProvider.GetConfiguration();
            var metadata = ConfigRequestProvider.GetMetadataIdentifier();

            var target = new UploadContext
            {
                IsValid = true,
                LinkedData = linkedData,
                Configuration = config,
                Metadata = metadata,
                FileContent = uploadStream,
                Context = _globalContext,
            };

            var metadataConfig =
                _globalContext.GetComponent<IMetadataConfigurationProvider>()
                    .GetMetadataConfiguration(ConfigRequestProvider.GetConfiguration());

            metadataConfig.MoveWorkflow(metadata, metadataConfig.GetExtensionPointValue(ConfigRequestProvider, "Upload"),
                target);

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