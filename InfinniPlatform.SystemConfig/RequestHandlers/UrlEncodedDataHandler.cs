using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    public sealed class UrlEncodedDataHandler : IWebRoutingHandler
    {
        public UrlEncodedDataHandler(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }


        public dynamic Process(dynamic parameters)
        {
            var сonfiguration = ConfigRequestProvider.GetConfiguration();
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();

            var target = new UrlEncodedDataContext
            {
                IsValid = true,
                FormData = parameters,
                Configuration = сonfiguration,
                Metadata = documentType
            };

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(сonfiguration);

            if (ExecuteExtensionPoint(metadataConfiguration, documentType, "ProcessUrlEncodedData", target))
            {
                dynamic item = new DynamicWrapper();
                item.IsValid = true;
                item.ValidationMessage = Resources.UrlEncodedDataProcessingComplete;

                dynamic result = new DynamicWrapper();

                if (target.Result != null)
                {
                    result.Data = target.Result.Data;
                    result.Info = target.Result.Info;
                }
                else
                {
                    item.ValidationMessage = "Content not found.";
                    item.IsValid = false;
                }

                item.Result = result;

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