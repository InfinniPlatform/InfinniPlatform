using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Implementation.Handlers;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Executors
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(Func<ApplyChangesHandler> applyChangesHandlerFactory)
        {
            _applyChangesHandlerFactory = applyChangesHandlerFactory;
        }


        private readonly Func<ApplyChangesHandler> _applyChangesHandlerFactory;


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

            var applyChangesHandler = _applyChangesHandlerFactory();
            applyChangesHandler.ConfigRequestProvider = new LocalDataProvider("RestfulApi", "configuration", "setdocument");

            return applyChangesHandler.ApplyJsonObject(null, request);
        }

        private static IEnumerable<object>[] GetBatches(IEnumerable<object> items, int batchSize)
        {
            return items.Select((item, index) => new { item, index })
                        .GroupBy(x => x.index / batchSize)
                        .Select(g => g.Select(x => x.item))
                        .ToArray();
        }
    }
}