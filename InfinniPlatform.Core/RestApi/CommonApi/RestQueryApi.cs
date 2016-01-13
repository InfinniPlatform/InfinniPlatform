using System;
using System.Collections.Generic;
using System.IO;

using InfinniPlatform.Core.Hosting;
using InfinniPlatform.Core.Index;
using InfinniPlatform.Core.Properties;
using InfinniPlatform.Sdk.Registers;

namespace InfinniPlatform.Core.RestApi.CommonApi
{
    [Obsolete]
    public sealed class RestQueryApi
    {
        public RestQueryApi(RestQueryBuilderFactory queryBuilderFactory)
        {
            _queryBuilderFactory = queryBuilderFactory;
        }

        private readonly RestQueryBuilderFactory _queryBuilderFactory;

        public RestQueryResponse QueryPostFile(string configuration, string documentType, string action, object linkedData, string filePath)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryPostFile(linkedData, filePath);

            CheckResponse(response);

            return response;
        }

        public RestQueryResponse QueryPostFile(string configuration, string documentType, string action, object linkedData, Stream fileStream)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryPostFile(linkedData, fileStream);

            CheckResponse(response);

            return response;
        }

        public RestQueryResponse QueryGetUrlEncodedData(string configuration, string documentType, string action, object linkedData)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryGetUrlEncodedData(linkedData);

            CheckResponse(response);

            return response;
        }

        public RestQueryResponse QueryPostJsonRaw(string configuration, string documentType, string action, string id, object body)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryPostJson(id, body);

            CheckResponse(response);

            return response;
        }

        public RestQueryResponse QueryGetRaw(string configuration, string documentType, string action, IEnumerable<object> filter, int pageNumber, int pageSize)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryGet(filter, pageNumber, pageSize, 1);

            CheckResponse(response);

            return response;
        }

        public RestQueryResponse QueryAggregationRaw(string configuration, string documentType, string action, string aggregationConfiguration, string aggregationdocumentType, IEnumerable<object> filterCriteria,
                                                     IEnumerable<dynamic> dimensions, IEnumerable<AggregationType> aggregationTypes, IEnumerable<string> aggregationFields, int pageNumber, int pageSize)
        {
            var builder = _queryBuilderFactory(configuration, documentType, action);

            var response = builder.QueryAggregation(
                aggregationConfiguration,
                aggregationdocumentType,
                filterCriteria,
                dimensions,
                aggregationTypes,
                aggregationFields,
                pageNumber,
                pageSize);

            CheckResponse(response);

            return response;
        }

        private static void CheckResponse(RestQueryResponse response)
        {
            if (!response.IsAllOk)
            {
                var errorMessage = response.Content;

                if (string.IsNullOrEmpty(errorMessage))
                {
                    if (response.IsRemoteServerNotFound)
                    {
                        errorMessage = Resources.RemoteServerNotFound;
                    }
                    else if (response.IsServiceNotRegistered)
                    {
                        errorMessage = Resources.ServiceNotRegistered;
                    }
                    else if (response.IsBusinessLogicError)
                    {
                        errorMessage = Resources.BusinessLogicError;
                    }
                    else if (response.IsServerError)
                    {
                        errorMessage = Resources.InternalServerError;
                    }
                }

                throw new ArgumentException(errorMessage);
            }
        }
    }
}