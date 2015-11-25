using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using InfinniPlatform.Api.LocalRouting;
using InfinniPlatform.Api.RestQuery.EventObjects;
using InfinniPlatform.Api.RestQuery.EventObjects.EventSerializers;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.Environment.Profiling;
using InfinniPlatform.Sdk.Events;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Api.RestQuery.RestQueryBuilders
{
    public sealed class LocalQueryBuilder : IRestQueryBuilder
    {
        public LocalQueryBuilder(string configuration, string metadata, string action, string userName,
                                 Func<string, string, string, dynamic, IOperationProfiler> operationProfiler)
        {
            _configuration = configuration;
            _metadata = metadata;
            _action = action;
            _userName = userName;
            _operationProfiler = operationProfiler;
        }

        private readonly string _action;
        private readonly string _configuration;
        private readonly string _metadata;
        private readonly Func<string, string, string, dynamic, IOperationProfiler> _operationProfiler;
        private readonly string _userName;

        public RestQueryResponse QueryGet(IEnumerable<object> filterObject, int pageNumber, int pageSize,
                                          int searchType = 1)
        {
            var searchBody = new Dictionary<string, object>
                             {
                                 {
                                     "FilterObject", filterObject != null
                                                         ? filterObject.Select(f => f.ToDynamic()).ToList()
                                                         : null
                                 },
                                 { "PageNumber", pageNumber },
                                 { "PageSize", pageSize },
                                 { "SearchType", searchType }
                             };

            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationGet(_configuration, _metadata, _action, searchBody, _userName); }, searchBody);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryPostFile(object linkedData, Stream file)
        {
            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationUpload(_configuration, _metadata, _action, linkedData, file, _userName); }, null);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryPostUrlEncodedData(object linkedData)
        {
            throw new NotImplementedException("Can't make multipart data operations on server side");
        }

        public RestQueryResponse QueryGetUrlEncodedData(dynamic linkedData)
        {
            string result = null;

            object body = DynamicWrapperExtensions.ToDynamic(linkedData);

            ExecuteProfiledOperation(
                                     () =>
                                     {
                                         result = RequestLocal.InvokeRestOperationDownload(_configuration, _metadata, _action, body,
                                                                                           _userName);
                                     }, body);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryPost(string id, object changesObject)
        {
            IEnumerable<EventDefinition> events = new List<EventDefinition>();

            if (changesObject != null)
            {
                var customSerializer = changesObject as IObjectToEventSerializer;
                events = customSerializer != null
                             ? customSerializer.GetEvents()
                             : new ObjectToEventSerializerStandard(changesObject).GetEvents();
            }

            var body = new Dictionary<string, object>
                       {
                           { "id", id },
                           { "events", events }
                       };

            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action, body, _userName); }, body);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryPostFile(object linkedData, string filePath)
        {
            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationUpload(_configuration, _metadata, _action, linkedData, filePath, _userName); }, null);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryAggregation(string aggregationConfiguration, string aggregationMetadata,
                                                  IEnumerable<object> filterObject, IEnumerable<object> dimensions,
                                                  IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber,
                                                  int pageSize)
        {
            var searchBody = new Dictionary<string, object>
                             {
                                 { "AggregationConfiguration", aggregationConfiguration },
                                 { "AggregationMetadata", aggregationMetadata },
                                 { "FilterObject", filterObject },
                                 { "Dimensions", dimensions },
                                 { "AggregationTypes", aggregationTypes },
                                 { "AggregationFields", aggregationFields },
                                 { "PageNumber", pageNumber },
                                 { "PageSize", pageSize }
                             };

            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationGet(_configuration, _metadata, _action, searchBody, _userName); }, searchBody);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryNotify(string metadataConfigurationId)
        {
            var body = new Dictionary<string, object>
                       {
                           { "version", "" },
                           { "metadataConfigurationId", metadataConfigurationId }
                       };

            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action, body, _userName); }, body);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        public RestQueryResponse QueryPostJson(string id, object jsonObject)
        {
            var body = new Dictionary<string, object>
                       {
                           { "id", id },
                           { "changesObject", jsonObject }
                       };

            string result = null;
            ExecuteProfiledOperation(
                                     () => { result = RequestLocal.InvokeRestOperationPost(_configuration, _metadata, _action, body, _userName); }, body);

            return new RestQueryResponse
                   {
                       Content = result,
                       HttpStatusCode = HttpStatusCode.OK
                   };
        }

        private void ExecuteProfiledOperation(Action operation, dynamic body)
        {
            dynamic bodyObject = null;
            if (body != null)
            {
                bodyObject = JObject.FromObject(body).ToString();
            }
            if (_operationProfiler != null)
            {
                var profiler = _operationProfiler(_configuration, _metadata, _action, bodyObject);
                profiler.Reset();
                operation();
                profiler.TakeSnapshot();
            }
            else
            {
                operation();
            }
        }
    }
}