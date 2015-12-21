using System;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.Api.SearchOptions.Builders;
using InfinniPlatform.Sdk.Dynamic;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public class DocumentApi
    {
        public DocumentApi(RestQueryApi restQueryApi,
                           ISetDocumentExecutor setDocumentExecutor,
                           IGetDocumentExecutor getDocumentExecutor)
        {
            _restQueryApi = restQueryApi;
            _setDocumentExecutor = setDocumentExecutor;
            _getDocumentExecutor = getDocumentExecutor;
        }

        private readonly RestQueryApi _restQueryApi;
        private readonly ISetDocumentExecutor _setDocumentExecutor;
        private readonly IGetDocumentExecutor _getDocumentExecutor;

        public IEnumerable<dynamic> GetDocumentByQuery(string queryText, bool denormalizeResult = false)
        {
            return _getDocumentExecutor.GetDocumentByQuery(queryText, denormalizeResult);
        }

        public dynamic GetDocument(string id)
        {
            return _getDocumentExecutor.GetDocument(id);
        }

        public int GetNumberOfDocuments(string configuration, string metadata, dynamic filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configuration, metadata, filter);
        }

        public int GetNumberOfDocuments(string configuration, string metadata, Action<FilterBuilder> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configuration, metadata, filter);
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            //var document = _getDocumentExecutor.GetDocument(configuration, metadata, filter, pageNumber, pageSize);

            var document = _getDocumentExecutor.GetDocumentUnfolded(configuration, metadata, filter, pageNumber, pageSize);

            return document;
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configuration, metadata, filter, pageNumber, pageSize, null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string metadata, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configuration, metadata, filter, pageNumber, pageSize, ignoreResolve, sorting);
        }

        public dynamic CreateDocument(string configuration, string metadata)
        {
            var result = ExecutePost("createdocument", null, new
                                                             {
                                                                 Configuration = configuration,
                                                                 Metadata = metadata
                                                             });

            return result.ToDynamic();
        }

        public dynamic DeleteDocument(string configuration, string metadata, string documentId)
        {
            var result = ExecutePost("deletedocument", null, new
                                                             {
                                                                 Configuration = configuration,
                                                                 Metadata = metadata,
                                                                 Id = documentId,
                                                                 Secured = false
                                                             });

            return result.ToDynamic();
        }

        public dynamic UpdateDocument(string configuration, string metadata, dynamic item, bool ignoreWarnings = false, bool allowNonSchemaProperties = false)
        {
            object transactionMarker = ObjectHelper.GetProperty(item, "TransactionMarker");

            if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
            {
                item.TransactionMarker = null;
            }

            var result = ExecutePost("updatedocument", item.Id, new
                                                                {
                                                                    Configuration = configuration,
                                                                    Metadata = metadata,
                                                                    IgnoreWarnings = ignoreWarnings,
                                                                    AllowNonSchemaProperties = allowNonSchemaProperties,
                                                                    Document = item.ChangesObject,
                                                                    TransactionMarker = transactionMarker,
                                                                    Secured = false
                                                                });

            return result.ToDynamic();
        }

        public dynamic SetDocument(string configuration, string documentType, dynamic documentInstance, bool ignoreWarnings = false, bool allowNonSchemaProperties = false)
        {
            return _setDocumentExecutor.SetDocument(configuration, documentType, documentInstance, ignoreWarnings, allowNonSchemaProperties);
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances, int batchSize = 200, bool allowNonSchemaProperties = false)
        {
            return _setDocumentExecutor.SetDocuments(configuration, documentType, documentInstances, batchSize, allowNonSchemaProperties);
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}