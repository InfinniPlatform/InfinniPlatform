using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// Реализует REST-клиент для DocumentApi.
    /// </summary>
    [Obsolete("Use InfinniPlatform.Sdk.RestApi.Documents.DocumentHttpServiceClient")]
    public sealed class DocumentApiClient : BaseRestClient
    {
        public DocumentApiClient(string server, int port, bool synchronous = false, IJsonObjectSerializer serializer = null) : base(server, port, serializer)
        {
            _synchronous = synchronous;
        }

        private readonly bool _synchronous;

        public dynamic GetDocumentById(string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetDocumentById");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["DocumentId"] = documentType,
                                                          ["Id"] = instanceId
                                                      }
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public IEnumerable<dynamic> GetDocument(string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return GetDocuments(documentType, filter.ToFilterCriterias(), pageNumber, pageSize, sorting.ToSortingCriterias());
        }

        public IEnumerable<dynamic> GetDocuments(string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Metadata"] = documentType,
                                                          ["Filter"] = filter,
                                                          ["PageNumber"] = pageNumber,
                                                          ["PageSize"] = pageSize,
                                                          ["Sorting"] = sorting
                                                      }
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public dynamic SetDocument(string documentType, object document)
        {
            return SetDocuments(documentType, new[] { document });
        }

        public dynamic SetDocuments(string documentType, IEnumerable<object> documents)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/SetDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Metadata"] = documentType,
                                                          ["Documents"] = documents
                                                      },
                                  ["Synchronous"] = _synchronous
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public dynamic DeleteDocument(string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/DeleteDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Metadata"] = documentType,
                                                          ["Id"] = instanceId
                                                      },
                                  ["Synchronous"] = _synchronous
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public long GetNumberOfDocuments(string documentType, Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(documentType, filter.ToFilterCriterias());
        }

        public long GetNumberOfDocuments(string documentType, IEnumerable<FilterCriteria> filter)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetNumberOfDocuments");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Metadata"] = documentType,
                                                          ["Filter"] = filter
                                                      }
                              };

            dynamic result = RequestExecutor.PostObject(requestUri, requestData);

            return (result != null) ? result.NumberOfDocuments : 0;
        }

        public void AttachFile(string documentType, string documentId, string fileProperty, string fileName, string fileType, Stream fileStream)
        {
            var requestUri = BuildRequestUri("/RestfulApi/Upload/configuration/UploadBinaryContent");

            var requestData = new DynamicWrapper
                              {
                                  ["Metadata"] = documentType,
                                  ["DocumentId"] = documentId,
                                  ["FieldName"] = fileProperty,
                                  ["Synchronous"] = _synchronous
                              };

            var pathArguments = $"/?linkedData={Uri.EscapeDataString(Serializer.ConvertToString(requestData))}";

            RequestExecutor.PostFile(requestUri + pathArguments, fileName, fileType, fileStream);
        }
    }
}