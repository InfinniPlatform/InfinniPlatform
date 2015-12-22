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

        public int GetNumberOfDocuments(string configurationName, string documentType, dynamic filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            var document = _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting: sorting);

            return document;
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, ignoreResolve, sorting);
        }

        public dynamic CreateDocument(string configurationName, string documentType)
        {
            var result = ExecutePost("createdocument", null, new
                                                             {
                                                                 Configuration = configurationName,
                                                                 Metadata = documentType
                                                             });

            return result.ToDynamic();
        }

        public dynamic DeleteDocument(string configurationName, string documentType, string documentId)
        {
            var result = ExecutePost("deletedocument", null, new
                                                             {
                                                                 Configuration = configurationName,
                                                                 Metadata = documentType,
                                                                 Id = documentId,
                                                                 Secured = false
                                                             });

            return result.ToDynamic();
        }

        public dynamic UpdateDocument(string configurationName, string documentType, dynamic item, bool ignoreWarnings = false, bool allowNonSchemaProperties = false)
        {
            object transactionMarker = ObjectHelper.GetProperty(item, "TransactionMarker");

            if (transactionMarker != null && !string.IsNullOrEmpty(transactionMarker.ToString()))
            {
                item.TransactionMarker = null;
            }

            var result = ExecutePost("updatedocument", item.Id, new
                                                                {
                                                                    Configuration = configurationName,
                                                                    Metadata = documentType,
                                                                    IgnoreWarnings = ignoreWarnings,
                                                                    AllowNonSchemaProperties = allowNonSchemaProperties,
                                                                    Document = item.ChangesObject,
                                                                    TransactionMarker = transactionMarker,
                                                                    Secured = false
                                                                });

            return result.ToDynamic();
        }

        public dynamic SetDocument(string configurationName, string documentType, object documentInstance)
        {
            return _setDocumentExecutor.SetDocument(configurationName, documentType, documentInstance);
        }

        public dynamic SetDocuments(string configurationName, string documentType, IEnumerable<object> documentInstances)
        {
            return _setDocumentExecutor.SetDocuments(configurationName, documentType, documentInstances);
        }

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}