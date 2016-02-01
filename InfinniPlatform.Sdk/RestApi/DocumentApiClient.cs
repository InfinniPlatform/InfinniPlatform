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
    public sealed class DocumentApiClient : BaseRestClient
    {
        public DocumentApiClient(string server, int port, bool synchronous = false) : base(server, port)
        {
            _synchronous = synchronous;
        }

        private readonly bool _synchronous;

        public dynamic GetDocumentById(string configuration, string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetDocumentById");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["ConfigId"] = configuration,
                                                          ["DocumentId"] = documentType,
                                                          ["Id"] = instanceId
                                                      }
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public IEnumerable<dynamic> GetDocument(string configuration, string documentType, Action<FilterBuilder> filter, int pageNumber, int pageSize, Action<SortingBuilder> sorting = null)
        {
            return GetDocuments(configuration, documentType, filter.ToFilterCriterias(), pageNumber, pageSize, sorting.ToSortingCriterias());
        }

        public IEnumerable<dynamic> GetDocuments(string configuration, string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Filter"] = filter,
                                                          ["PageNumber"] = pageNumber,
                                                          ["PageSize"] = pageSize,
                                                          ["Sorting"] = sorting
                                                      }
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public dynamic SetDocument(string configuration, string documentType, object document)
        {
            return SetDocuments(configuration, documentType, new[] { document });
        }

        public dynamic SetDocuments(string configuration, string documentType, IEnumerable<object> documents)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/SetDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Documents"] = documents
                                                      },
                                  ["Synchronous"] = _synchronous
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public dynamic DeleteDocument(string configuration, string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/DeleteDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Id"] = instanceId
                                                      },
                                  ["Synchronous"] = _synchronous
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public long GetNumberOfDocuments(string configuration, string documentType, Action<FilterBuilder> filter)
        {
            return GetNumberOfDocuments(configuration, documentType, filter.ToFilterCriterias());
        }

        public long GetNumberOfDocuments(string configuration, string documentType, IEnumerable<FilterCriteria> filter)
        {
            var requestUri = BuildRequestUri("/RestfulApi/StandardApi/configuration/GetNumberOfDocuments");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Filter"] = filter
                                                      }
                              };

            dynamic result = RequestExecutor.PostObject(requestUri, requestData);

            return (result != null) ? result.NumberOfDocuments : 0;
        }

        public void AttachFile(string configuration, string documentType, string documentId, string fileProperty, Stream fileStream)
        {
            var requestUri = BuildRequestUri("/RestfulApi/Upload/configuration/UploadBinaryContent");

            var requestData = new DynamicWrapper
                              {
                                  ["Configuration"] = configuration,
                                  ["Metadata"] = documentType,
                                  ["DocumentId"] = documentId,
                                  ["FieldName"] = fileProperty,
                                  ["Synchronous"] = _synchronous
                              };

            var pathArguments = $"/?linkedData={Uri.EscapeDataString(JsonObjectSerializer.Default.ConvertToString(requestData))}";

            RequestExecutor.PostFile(requestUri + pathArguments, fileProperty, fileStream);
        }
    }
}