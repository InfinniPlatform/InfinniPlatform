using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Dynamic;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class DocumentApi : IDocumentApi
    {
        public dynamic CreateSession()
        {
            return new SessionApi().CreateSession();
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

            return new SessionApi().Attach(session, document);
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

            new SessionApi().AttachFile(linkedData, fileStream);
        }

        public void DetachFile(string session, string instanceId, string fieldName)
        {
            dynamic body = new
            {
                InstanceId = instanceId,
                FieldName = fieldName,
                SessionId = session
            };

            new SessionApi().DetachFile(body);
        }

        public dynamic Detach(string session, string instanceId)
        {
            return new SessionApi().Detach(session, instanceId);
        }

        public dynamic RemoveSession(string sessionId)
        {
            return new SessionApi().RemoveSession(sessionId);
        }

        public dynamic GetSession(string sessionId)
        {
            return new SessionApi().GetSession(sessionId);
        }

        public dynamic SaveSession(string sessionId)
        {
            return new SessionApi().SaveSession(sessionId);
        }

        public dynamic GetDocumentById(string applicationId, string documentType, string instanceId)
        {
            return new RestApi.DataApi.DocumentApi().GetDocument(instanceId);
        }

        public IEnumerable<dynamic> GetDocument(string applicationId, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {          
            return new RestApi.DataApi.DocumentApi().GetDocument(applicationId, documentType, new FilterConverter().ConvertToInternal(filter), pageNumber,
                pageSize,null, new SortingConverter().ConvertToInternal(sorting));
        }

        public long GetNumberOfDocuments(string applicationId, string documentType, Action<FilterBuilder> filter)
        {
            return new DataApi.DocumentApi().GetNumberOfDocuments(applicationId, documentType, new FilterConverter().ConvertToInternal(filter));
        }

        public dynamic SetDocument(string applicationId, string documentType, object document)
        {

            return new DataApi.DocumentApi().SetDocument(applicationId, documentType, document);
        }

        public dynamic SetDocuments(string applicationId, string documentType, IEnumerable<object> documents)
        {
            return new DataApi.DocumentApi().SetDocuments(applicationId, documentType, documents);
        }

        public void UpdateDocument(string applicationId, string documentType, string instanceId, object changesObject)
        {
            new DataApi.DocumentApi().UpdateDocument(applicationId, documentType, changesObject);
        }

        public dynamic DeleteDocument(string applicationId, string documentType, string instanceId)
        {
            return new DataApi.DocumentApi().DeleteDocument(applicationId, documentType, instanceId);
        }


    }
}
