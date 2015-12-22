using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.ContextTypes.ContextImpl;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Implementation.Handlers;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Contracts;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Transactions;
using InfinniPlatform.Sdk.IoC;

namespace InfinniPlatform.RestfulApi.Executors
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(IGlobalContext globalContext, Func<IContainerResolver> containerResolver, IMetadataConfigurationProvider metadataConfigurationProvider)
        {
            _globalContext = globalContext;
            _metadataConfigurationProvider = metadataConfigurationProvider;

            // TODO: Сделано именно так, потому что есть циклическая ссылка "SetDocumentExecutor - TransactionManager - BinaryManager - SetDocumentExecutor"
            _transactionManager = new Lazy<ITransactionManager>(() => containerResolver().Resolve<ITransactionComponent>().GetTransactionManager());
        }


        private readonly IGlobalContext _globalContext;
        private readonly IMetadataConfigurationProvider _metadataConfigurationProvider;
        private readonly Lazy<ITransactionManager> _transactionManager;


        public dynamic SetDocument(string configuration, string documentType, object documentInstance)
        {
            return ExecuteSetDocument(configuration, documentType, new[] { documentInstance });
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            var documentBatches = GetBatches(documentInstances, 100);

            foreach (var documents in documentBatches)
            {
                dynamic batchResult = ExecuteSetDocument(configuration, documentType, documents);

                if (batchResult?.IsValid == false)
                {
                    throw new ArgumentException(batchResult.ToString());
                }
            }

            dynamic result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = Resources.BatchCompletedSuccessfully;

            return result;
        }

        private object ExecuteSetDocument(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            dynamic request = new DynamicWrapper();
            request.Configuration = configuration;
            request.Metadata = documentType;
            request.Documents = documentInstances;
            request.IgnoreWarnings = false;
            request.AllowNonSchemaProperties = false;

            var configRequestProvider = new LocalDataProvider("RestfulApi", "configuration", "setdocument");

            return ApplyJsonObject(configRequestProvider, request);
        }

        private static IEnumerable<object>[] GetBatches(IEnumerable<object> items, int batchSize)
        {
            return items.Select((item, index) => new { item, index })
                        .GroupBy(x => x.index / batchSize)
                        .Select(g => g.Select(x => x.item))
                        .ToArray();
        }

        private dynamic ApplyJsonObject(IConfigRequestProvider configRequestProvider, dynamic changesObject)
        {
            var documentType = configRequestProvider.GetMetadataIdentifier();

            if (string.IsNullOrEmpty(documentType))
            {
                throw new ArgumentException("document type undefined");
            }

            // Метаданные конфигурации
            var metadataConfiguration = _metadataConfigurationProvider.GetMetadataConfiguration(configRequestProvider.GetConfiguration());

            // Идентификатор транзакции
            var transactionMarker = changesObject.TransactionMarker ?? Guid.NewGuid().ToString();

            // Начало транзакции
            var transaction = _transactionManager.Value.GetTransaction(transactionMarker);

            var moveContext = new ApplyContext
            {
                Id = changesObject.Id,
                Context = _globalContext,
                Item = changesObject,
                Type = documentType,
                Configuration = configRequestProvider.GetConfiguration(),
                Metadata = configRequestProvider.GetMetadataIdentifier(),
                Action = configRequestProvider.GetServiceName(),
                TransactionMarker = transactionMarker
            };

            if (!ExecuteExtensionPoint(configRequestProvider, metadataConfiguration, documentType, "Move", moveContext))
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(moveContext);
            }

            var targetResult = new ApplyResultContext
            {
                Context = _globalContext,
                Result = moveContext.Result ?? moveContext.Item,
                Item = moveContext.Item,
                Configuration = configRequestProvider.GetConfiguration(),
                Metadata = configRequestProvider.GetMetadataIdentifier(),
                Action = configRequestProvider.GetServiceName(),
            };

            if (!ExecuteExtensionPoint(configRequestProvider, metadataConfiguration, documentType, "GetResult", targetResult))
            {
                return AggregateExtensions.PrepareInvalidFilterAggregate(moveContext);
            }

            transaction.CommitTransaction();

            return AggregateExtensions.PrepareResultAggregate(targetResult.Result);
        }

        private bool ExecuteExtensionPoint(IConfigRequestProvider configRequestProvider, IMetadataConfiguration metadataConfiguration, string documentType, string extensionPointName, ICommonContext extensionPointContext)
        {
            var extensionPoint = metadataConfiguration.GetExtensionPointValue(configRequestProvider, extensionPointName);

            if (!string.IsNullOrEmpty(extensionPoint))
            {
                metadataConfiguration.MoveWorkflow(documentType, extensionPoint, extensionPointContext);
            }

            return extensionPointContext.IsValid;
        }
    }
}