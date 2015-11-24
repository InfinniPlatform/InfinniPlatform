using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class DocumentApi : IDocumentApi
    {
        public DocumentApi()
        {
            _sessionApi = new DataApi.SessionApi();
            _documentApi = new DataApi.DocumentApi();
            _filterConverter = new FilterConverter();
            _sortingConverter = new SortingConverter();
        }


        private readonly DataApi.SessionApi _sessionApi;
        private readonly DataApi.DocumentApi _documentApi;
        private readonly FilterConverter _filterConverter;
        private readonly SortingConverter _sortingConverter;


        public dynamic CreateSession()
        {
            return _sessionApi.CreateSession();
        }

        public dynamic Attach(string session, string application, string documentType, string instanceId, dynamic document)
        {
            dynamic changesObject = JObject.FromObject(new
            {
                Application = application,
                DocumentType = documentType,
                Document = document
            });

            changesObject.Document.Id = instanceId;

            return _sessionApi.Attach(session, document);
        }

        public void AttachFile(string session, string application, string documentType, string instanceId, string fieldName, string fileName, Stream fileStream)
        {
            var linkedData = new
            {
                Configuration = application,
                Metadata = documentType,
                InstanceId = instanceId,
                FieldName = fieldName,
                FileName = fileName,
                SessionId = session
            };

            _sessionApi.AttachFile(linkedData, fileStream);
        }

        public void DetachFile(string session, string instanceId, string fieldName)
        {
            dynamic body = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                SessionId = session
            };

            _sessionApi.DetachFile(body);
        }

        public dynamic Detach(string session, string instanceId)
        {
            return _sessionApi.Detach(session, instanceId);
        }

        public dynamic RemoveSession(string sessionId)
        {
            return _sessionApi.RemoveSession(sessionId);
        }

        public dynamic GetSession(string sessionId)
        {
            return _sessionApi.GetSession(sessionId);
        }

        public dynamic SaveSession(string sessionId)
        {
            return _sessionApi.SaveSession(sessionId);
        }

        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            return _documentApi.GetDocument(instanceId);
        }

        public IEnumerable<dynamic> GetDocument(string applicationId, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return _documentApi.GetDocument(
                applicationId,
                documentType,
                _filterConverter.ConvertToInternal(filter),
                pageNumber,
                pageSize,
                null,
                _sortingConverter.ConvertToInternal(sorting));
        }

        public long GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter)
        {
            return _documentApi.GetNumberOfDocuments(applicationId, documentType, _filterConverter.ConvertToInternal(filter));
        }

        public dynamic SetDocument(string applicationId, string documentType, object document)
        {
            return _documentApi.SetDocument(applicationId, documentType, document);
        }

        public dynamic SetDocuments(string applicationId, string documentType, IEnumerable<object> documents)
        {
            return _documentApi.SetDocuments(applicationId, documentType, documents);
        }

        public void UpdateDocument(string applicationId, string documentType, string instanceId, object changesObject)
        {
            _documentApi.UpdateDocument(applicationId, documentType, changesObject);
        }

        public dynamic DeleteDocument(string applicationId, string documentType, string instanceId)
        {
            return _documentApi.DeleteDocument(applicationId, documentType, instanceId);
        }
    }
}