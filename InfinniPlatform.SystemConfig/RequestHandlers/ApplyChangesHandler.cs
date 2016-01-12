using InfinniPlatform.Core.ContextTypes;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Metadata;
using InfinniPlatform.Sdk.Contracts;

namespace InfinniPlatform.SystemConfig.RequestHandlers
{
    /// <summary>
    /// Обработчик HTTP-запросов обработки событийной модели.
    /// </summary>
    public sealed class ApplyChangesHandler : IWebRoutingHandler
    {
        public ApplyChangesHandler(IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;


        public IConfigRequestProvider ConfigRequestProvider { get; set; }


        /// <summary>
        /// Применить изменения, представленные в виде объекта
        /// </summary>
        /// <param name="id">Идентификатор объекта, к которому следует применить изменения</param>
        /// <param name="changesObject">Объект JSON, содержащий события изменения объекта</param>
        /// <returns>Результат обработки JSON объекта</returns>
        public dynamic ApplyJsonObject(string id, dynamic changesObject)
        {
            var сonfiguration = ConfigRequestProvider.GetConfiguration();
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();
            var serviceName = ConfigRequestProvider.GetServiceName();

            changesObject.Documents = changesObject.Documents ?? new object[] { changesObject.Document };

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(сonfiguration);

            var moveContext = new ApplyContext
            {
                Id = changesObject.Id,
                Item = changesObject,
                Type = documentType,
                Configuration = сonfiguration,
                Metadata = documentType,
                Action = serviceName
            };

            if (!ExecuteExtensionPoint(metadataConfiguration, documentType, "Move", moveContext))
            {
                return AggregateExtensions.PrepareInvalidResult(moveContext);
            }

            var targetResult = new ApplyResultContext
            {
                Result = moveContext.Result ?? moveContext.Item,
                Item = moveContext.Item,
                Configuration = сonfiguration,
                Metadata = documentType,
                Action = serviceName
            };

            if (!ExecuteExtensionPoint(metadataConfiguration, documentType, "GetResult", targetResult))
            {
                return AggregateExtensions.PrepareInvalidResult(moveContext);
            }

            return targetResult.Result;
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