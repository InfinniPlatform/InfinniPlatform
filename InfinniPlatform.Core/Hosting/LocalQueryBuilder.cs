using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;

using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.RestApi.Auth;
using InfinniPlatform.Sdk.Dynamic;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.Hosting
{
    public delegate LocalQueryBuilder RestQueryBuilderFactory(string configuration, string documentType, string action);

    public sealed class LocalQueryBuilder
    {
        public LocalQueryBuilder(string configuration, string documentType, string action, IRequestLocal requestLocal)
        {
            _configuration = configuration;
            _documentType = documentType;
            _action = action;
            _requestLocal = requestLocal;

            _userName = string.IsNullOrEmpty(Thread.CurrentPrincipal.Identity.Name) ? AuthorizationStorageExtensions.UnknownUser : Thread.CurrentPrincipal.Identity.Name;
        }


        private readonly string _action;
        private readonly string _configuration;
        private readonly string _documentType;
        private readonly string _userName;
        private readonly IRequestLocal _requestLocal;


        public RestQueryResponse QueryGet(IEnumerable<object> filterObject, int pageNumber, int pageSize, int searchType = 1)
        {
            var searchBody = new Dictionary<string, object>
                             {
                                 { "FilterObject", filterObject?.Select(f => f.ToDynamic()).ToList() },
                                 { "PageNumber", pageNumber },
                                 { "PageSize", pageSize },
                                 { "SearchType", searchType }
                             };

            var result = _requestLocal.InvokeRestOperationGet(_configuration, _documentType, _action, searchBody, _userName);

            return new RestQueryResponse
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public RestQueryResponse QueryPostFile(object linkedData, Stream file)
        {
            var result = _requestLocal.InvokeRestOperationUpload(_configuration, _documentType, _action, linkedData, file, _userName);

            return new RestQueryResponse
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public RestQueryResponse QueryGetUrlEncodedData(dynamic linkedData)
        {
            object body = DynamicWrapperExtensions.ToDynamic(linkedData);

            var result = _requestLocal.InvokeRestOperationDownload(_configuration, _documentType, _action, body, _userName);

            return new RestQueryResponse
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }

        public RestQueryResponse QueryPostFile(object linkedData, string filePath)
        {
            var result = _requestLocal.InvokeRestOperationUpload(_configuration, _documentType, _action, linkedData, filePath, _userName);

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

            var result = _requestLocal.InvokeRestOperationGet(_configuration, _documentType, _action, searchBody, _userName);

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

            var result = _requestLocal.InvokeRestOperationPost(_configuration, _documentType, _action, body, _userName);

            return new RestQueryResponse
            {
                Content = result,
                HttpStatusCode = HttpStatusCode.OK
            };
        }
    }
}