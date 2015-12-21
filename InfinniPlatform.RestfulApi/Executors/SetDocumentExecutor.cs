using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.Api.Linq;
using InfinniPlatform.Api.Properties;
using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.RestfulApi.Executors
{
    internal class SetDocumentExecutor : ISetDocumentExecutor
    {
        public SetDocumentExecutor(RestQueryApi restQueryApi)
        {
            _restQueryApi = restQueryApi;
        }

        private readonly RestQueryApi _restQueryApi;

        public dynamic SetDocument(string configuration, string documentType, dynamic documentInstance, bool ignoreWarnings = false, bool allowNonSchemaProperties = false)
        {
            object transactionMarker = ObjectHelper.GetProperty(documentInstance, "TransactionMarker");

            if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
            {
                documentInstance.TransactionMarker = null;
            }

            var result = ExecutePost("setdocument", null, new
            {
                Configuration = configuration,
                Metadata = documentType,
                IgnoreWarnings = ignoreWarnings,
                AllowNonSchemaProperties = allowNonSchemaProperties,
                Document = documentInstance,
                TransactionMarker = transactionMarker,
                Secured = false
            });

            return result.ToDynamic();
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances, int batchSize = 200, bool allowNonSchemaProperties = false)
        {
            var batches = documentInstances.Batch(batchSize);

            foreach (var batch in batches)
            {
                object transactionMarker = null;

                foreach (dynamic document in batch.ToArray())
                {
                    transactionMarker = ObjectHelper.GetProperty(document, "TransactionMarker");

                    if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
                    {
                        document.TransactionMarker = null;
                    }
                }

                var response = ExecutePost("setdocument", null, new
                {
                    Configuration = configuration,
                    Metadata = documentType,
                    Documents = batch,
                    AllowNonSchemaProperties = allowNonSchemaProperties,
                    TransactionMarker = transactionMarker,
                    Secured = false
                });

                if (!string.IsNullOrEmpty(response.Content))
                {
                    dynamic dynamicContent = response.ToDynamic();

                    if (dynamicContent != null &&
                        dynamicContent.IsValid != null &&
                        dynamicContent.IsValid == false)
                    {
                        throw new ArgumentException(response.Content);
                    }
                }
            }

            dynamic result = new DynamicWrapper();
            result.IsValid = true;
            result.ValidationMessage = Resources.BatchCompletedSuccessfully;

            return result;
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}