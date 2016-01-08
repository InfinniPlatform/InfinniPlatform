using System.IO;

using InfinniPlatform.Core.ContextTypes.ContextImpl;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    public sealed class UploadHandler : IWebRoutingHandler
    {
        public UploadHandler(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }


        public dynamic UploadFile(dynamic linkedData, Stream uploadStream)
        {
            var сonfiguration = ConfigRequestProvider.GetConfiguration();
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();

            var target = new UploadContext
            {
                IsValid = true,
                LinkedData = linkedData,
                Configuration = сonfiguration,
                Metadata = documentType,
                FileContent = uploadStream
            };

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(сonfiguration);

            if (ExecuteExtensionPoint(metadataConfiguration, documentType, "Upload", target))
            {
                dynamic item = new DynamicWrapper();
                item.IsValid = true;
                item.ValidationMessage = Resources.DocumentContentSuccessfullyUploaded;
                item.Result = target.Result;

                return item;
            }

            return AggregateExtensions.PrepareInvalidResult(target);
        }

        private bool ExecuteExtensionPoint(IMetadataConfiguration metadataConfiguration, string documentType, string extensionPointName, ICommonContext extensionPointContext)
        {
            var extensionPoint = metadataConfiguration.GetExtensionPointValue(ConfigRequestProvider, extensionPointName);

            if (!string.IsNullOrEmpty(extensionPoint))
            {
                metadataConfiguration.MoveWorkflow(documentType, extensionPoint, extensionPointContext);
            }

            return extensionPointContext.IsValid;
        }
    }
}