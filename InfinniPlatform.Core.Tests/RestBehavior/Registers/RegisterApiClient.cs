using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.Core.Tests.RestBehavior.Registers
{
    internal sealed class RegisterApiClient : BaseRestClient
    {
        public RegisterApiClient(string server, int port) : base(server, port)
        {
        }

        public IEnumerable<object> GetEntries(
            string configuration,
            string registerName,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize)
        {
            var requestUri = BuildRequestUri("/System/Registers/GetEntries");

            var requestData = new RegisterApiRequest
                              {
                                  Configuration = configuration,
                                  RegisterName = registerName,
                                  Filter = filter,
                                  PageNumber = pageNumber,
                                  PageSize = pageSize
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public IEnumerable<object> GetValuesByDate(
            string configuration,
            string registerName,
            DateTime aggregationDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null)
        {
            var requestUri = BuildRequestUri("/System/Registers/GetValuesByDate");

            var requestData = new RegisterApiRequest
                              {
                                  Configuration = configuration,
                                  RegisterName = registerName,
                                  BeginDate = aggregationDate,
                                  EndDate = aggregationDate,
                                  Filter = filter,
                                  DimensionsProperties = dimensionsProperties,
                                  ValueProperties = valueProperties,
                                  AggregationTypes = aggregationTypes
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public IEnumerable<object> GetValuesBetweenDates(
            string configuration,
            string registerName,
            DateTime beginDate,
            DateTime endDate,
            IEnumerable<FilterCriteria> filter = null,
            IEnumerable<string> dimensionsProperties = null,
            IEnumerable<string> valueProperties = null,
            IEnumerable<AggregationType> aggregationTypes = null)
        {
            var requestUri = BuildRequestUri("/System/Registers/GetValuesBetweenDates");

            var requestData = new RegisterApiRequest
                              {
                                  Configuration = configuration,
                                  RegisterName = registerName,
                                  BeginDate = beginDate,
                                  EndDate = endDate,
                                  Filter = filter,
                                  DimensionsProperties = dimensionsProperties,
                                  ValueProperties = valueProperties,
                                  AggregationTypes = aggregationTypes
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public void RecalculateTotals(
            string configuration)
        {
            var requestUri = BuildRequestUri("/System/Registers/RecalculateTotals");

            var requestData = new RegisterApiRequest
                              {
                                  Configuration = configuration
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }
    }
}