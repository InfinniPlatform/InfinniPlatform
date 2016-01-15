using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Serialization;

namespace InfinniPlatform.Sdk.RestApi
{
    /// <summary>
    /// REST-клиент для DocumentApi.
    /// </summary>
    public sealed class InfinniDocumentApi : BaseApi
    {
        public InfinniDocumentApi(string server, int port) : base(server, port)
        {
        }

        public dynamic GetDocumentById(string configuration, string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/SystemConfig/StandardApi/configuration/GetDocumentById");

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
            IEnumerable<FilterCriteria> filterCriterias = null;

            if (filter != null)
            {
                var filterBuilder = new FilterBuilder();
                filter(filterBuilder);

                filterCriterias = filterBuilder.CriteriaList;
            }

            IEnumerable<SortingCriteria> sortingCriterias = null;

            if (sorting != null)
            {
                var sortingBuilder = new SortingBuilder();
                sorting(sortingBuilder);

                sortingCriterias = sortingBuilder.SortingList;
            }

            return GetDocuments(configuration, documentType, filterCriterias, pageNumber, pageSize, sortingCriterias);
        }

        public IEnumerable<dynamic> GetDocuments(string configuration, string documentType, IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting = null)
        {
            var requestUri = BuildRequestUri("/SystemConfig/StandardApi/configuration/GetDocument");

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
            var requestUri = BuildRequestUri("/SystemConfig/StandardApi/configuration/SetDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Documents"] = documents
                                                      }
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public dynamic DeleteDocument(string configuration, string documentType, string instanceId)
        {
            var requestUri = BuildRequestUri("/SystemConfig/StandardApi/configuration/DeleteDocument");

            var requestData = new DynamicWrapper
                              {
                                  ["changesObject"] = new DynamicWrapper
                                                      {
                                                          ["Configuration"] = configuration,
                                                          ["Metadata"] = documentType,
                                                          ["Id"] = instanceId
                                                      }
                              };

            return RequestExecutor.PostObject(requestUri, requestData);
        }

        public long GetNumberOfDocuments(string configuration, string documentType, Action<FilterBuilder> filter)
        {
            IEnumerable<FilterCriteria> filterCriterias = null;

            if (filter != null)
            {
                var filterBuilder = new FilterBuilder();
                filter(filterBuilder);

                filterCriterias = filterBuilder.CriteriaList;
            }

            return GetNumberOfDocuments(configuration, documentType, filterCriterias);
        }

        public long GetNumberOfDocuments(string configuration, string documentType, IEnumerable<FilterCriteria> filter)
        {
            var requestUri = BuildRequestUri("/SystemConfig/StandardApi/configuration/GetNumberOfDocuments");

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

            return result?.NumberOfDocuments;
        }

        public void AttachFile(string configuration, string documentType, string documentId, string fileProperty, Stream fileStream)
        {
            var requestUri = BuildRequestUri("/SystemConfig/Upload/configuration/UploadBinaryContent");

            var requestData = new DynamicWrapper
                              {
                                  ["Configuration"] = configuration,
                                  ["Metadata"] = documentType,
                                  ["DocumentId"] = documentId,
                                  ["FieldName"] = fileProperty
                              };

            var pathArguments = $"/?linkedData={Uri.EscapeDataString(JsonObjectSerializer.Default.ConvertToString(requestData))}";

            RequestExecutor.PostFile(requestUri + pathArguments, fileProperty, fileStream);
        }
    }
}