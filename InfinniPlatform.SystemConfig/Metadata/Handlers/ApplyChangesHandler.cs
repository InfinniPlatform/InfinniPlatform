using System;

using InfinniPlatform.Core.ContextTypes.ContextImpl;
using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Environment.Metadata;

namespace InfinniPlatform.SystemConfig.Metadata.Handlers
{
    /// <summary>
    /// Обработчик HTTP-запросов обработки событийной модели.
    /// </summary>
    public sealed class ApplyChangesHandler : IWebRoutingHandler
    {
        public ApplyChangesHandler(IGlobalContext globalContext, IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _globalContext = globalContext;
            _metadataConfigurationProvider = metadataConfigurationProvider;
        }


        private readonly IGlobalContext _globalContext;
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
            var documentType = ConfigRequestProvider.GetMetadataIdentifier();

            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentException("document type undefined");
            }

            changesObject.Documents = changesObject.Documents ?? new object[] { changesObject.Document } ;

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(ConfigRequestProvider.GetConfiguration());

            // Идентификатор транзакции
            var transactionMarker = changesObject.TransactionMarker ?? Guid.NewGuid().ToString();

            var moveContext = new ApplyContext
            {
                Id = changesObject.Id,
                Context = _globalContext,
                Item = changesObject,
                Type = documentType,
                Configuration = ConfigRequestProvider.GetConfiguration(),
                Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
                Action = ConfigRequestProvider.GetServiceName(),
                TransactionMarker = transactionMarker
            };

            if (!ExecuteExtensionPoint(metadataConfiguration, documentType, "Move", moveContext))
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(moveContext);
            }

            var targetResult = new ApplyResultContext
            {
                Context = _globalContext,
                Result = moveContext.Result ?? moveContext.Item,
                Item = moveContext.Item,
                Configuration = ConfigRequestProvider.GetConfiguration(),
                Metadata = ConfigRequestProvider.GetMetadataIdentifier(),
                Action = ConfigRequestProvider.GetServiceName(),
            };

            if (!ExecuteExtensionPoint(metadataConfiguration, documentType, "GetResult", targetResult))
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(moveContext);
            }

            return AggregateExtensions.PrepareResultAggregate(targetResult.Result);
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