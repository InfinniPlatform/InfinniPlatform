using System;
using System.Collections.Generic;

using InfinniPlatform.Api.RestApi.CommonApi;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;

using RestQueryResponse = InfinniPlatform.Api.RestQuery.RestQueryResponse;

namespace InfinniPlatform.Api.RestApi.DataApi
{
    public class DocumentApi : IDocumentApi
    {
        public DocumentApi(RestQueryApi restQueryApi,
                           ISetDocumentExecutor setDocumentExecutor,
                           IGetDocumentExecutor getDocumentExecutor)
        {
            _restQueryApi = restQueryApi;
            _setDocumentExecutor = setDocumentExecutor;
            _getDocumentExecutor = getDocumentExecutor;
            _filterConverter = new FilterConverter();
            _sortingConverter = new SortingConverter();
        }

        private readonly FilterConverter _filterConverter;
        private readonly IGetDocumentExecutor _getDocumentExecutor;
        private readonly RestQueryApi _restQueryApi;
        private readonly ISetDocumentExecutor _setDocumentExecutor;
        private readonly SortingConverter _sortingConverter;

        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            return GetDocument(instanceId);
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

        long IDocumentApi.GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(applicationId, documentType, _filterConverter.ConvertToInternal(filter));
        }

        IEnumerable<dynamic> IDocumentApi.GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return GetDocument(configurationName,
                               documentType,
                               _filterConverter.ConvertToInternal(filter),
                               pageNumber,
                               pageSize,
                               null,
                               _sortingConverter.ConvertToInternal(sorting));
        }

        public dynamic SetDocument(string configurationName, string documentType, object documentInstance)
        {
            return _setDocumentExecutor.SetDocument(configurationName, documentType, documentInstance);
        }

        public dynamic SetDocuments(string configurationName, string documentType, IEnumerable<object> documentInstances)
        {
            return _setDocumentExecutor.SetDocuments(configurationName, documentType, documentInstances);
        }

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

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<SearchOptions.Builders.FilterBuilder> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            var document = _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting: sorting);

            return document;
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<SearchOptions.Builders.FilterBuilder> filter, int pageNumber, int pageSize, Action<SearchOptions.Builders.SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, null, sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<SearchOptions.Builders.FilterBuilder> filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve, Action<SearchOptions.Builders.SortingBuilder> sorting = null)
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

        private RestQueryResponse ExecutePost(string action, string id, object body)
        {
            var response = _restQueryApi.QueryPostJsonRaw("RestfulApi", "configuration", action, id, body);

            return response;
        }
    }
}