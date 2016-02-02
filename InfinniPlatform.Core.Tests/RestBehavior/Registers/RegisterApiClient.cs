using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;
using InfinniPlatform.Sdk.Registers;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.Core.Tests.RestBehavior.Registers
{
    internal sealed class RegisterApiClient : BaseRestClient
    {
        public RegisterApiClient(string server, int port, bool synchronous = false) : base(server, port)
        {
            _synchronous = synchronous;
        }

        private readonly bool _synchronous;

        public IEnumerable<object> GetEntries(
            string registerName,
            IEnumerable<FilterCriteria> filter,
            int pageNumber,
            int pageSize)
        {
            var requestUri = BuildRequestUri("/System/Registers/GetEntries");

            var requestData = new RegisterApiRequest
                              {
                                  RegisterName = registerName,
                                  Filter = filter,
                                  PageNumber = pageNumber,
                                  PageSize = pageSize
                              };

            return RequestExecutor.PostArray(requestUri, requestData);
        }

        public IEnumerable<object> GetValuesByDate(
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

        public void RecalculateTotals()
        {
            var requestUri = BuildRequestUri("/System/Registers/RecalculateTotals");

            var requestData = new RegisterApiRequest
                              {
                                  Synchronous = _synchronous
                              };

            RequestExecutor.PostObject(requestUri, requestData);
        }
    }
}