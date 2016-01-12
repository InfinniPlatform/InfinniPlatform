using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;

namespace InfinniPlatform.Core.RestApi.DataApi
{
    public sealed class DocumentApi : IDocumentApi
    {
        public DocumentApi(ISetDocumentExecutor setDocumentExecutor, IGetDocumentExecutor getDocumentExecutor)
        {
            _setDocumentExecutor = setDocumentExecutor;
            _getDocumentExecutor = getDocumentExecutor;
        }

        private readonly IGetDocumentExecutor _getDocumentExecutor;
        private readonly ISetDocumentExecutor _setDocumentExecutor;
        
        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            return GetDocument(instanceId);
        }

        public dynamic DeleteDocument(string configuration, string documentType, string documentId)
        {
            return _setDocumentExecutor.DeleteDocument(configuration, documentType, documentId);
        }

        long IDocumentApi.GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(applicationId, documentType, filter);
        }

        public dynamic SetDocument(string configuration, string documentType, object documentInstance)
        {
            return _setDocumentExecutor.SaveDocument(configuration, documentType, documentInstance);
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documentInstances)
        {
            return _setDocumentExecutor.SaveDocuments(configuration, documentType, documentInstances);
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

        public int GetNumberOfDocuments(string configurationName, string documentType, Action<FilterBuilder> filter)
        {
            return _getDocumentExecutor.GetNumberOfDocuments(configurationName, documentType, filter);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, dynamic filter, int pageNumber, int pageSize, IEnumerable<dynamic> ignoreResolve = null, dynamic sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, sorting: sorting);
        }

        public IEnumerable<dynamic> GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, null, sorting);
        }

        IEnumerable<dynamic> IDocumentApi.GetDocument(string configurationName, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting)
        {
            return _getDocumentExecutor.GetDocument(configurationName, documentType, filter, pageNumber, pageSize, null, sorting);
        }
    }
}