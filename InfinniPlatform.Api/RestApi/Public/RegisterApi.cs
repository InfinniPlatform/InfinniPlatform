using System;
using System.Collections.Generic;

using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class RegisterApi : IRegisterApi
    {
        public RegisterApi()
        {
            _registerApi = new DataApi.RegisterApi();
            _filterConverter = new FilterConverter();
        }


        private readonly DataApi.RegisterApi _registerApi;
        private readonly FilterConverter _filterConverter;


        public IEnumerable<dynamic> GetValuesByDate(string configuration,
                                                    string register,
                                                    DateTime endDate,
                                                    IEnumerable<string> dimensions = null,
                                                    IEnumerable<string> valueProperties = null,
                                                    IEnumerable<AggregationType> valueAggregationTypes = null,
                                                    Action<FilterBuilder> filter = null)
        {
            return _registerApi.GetValuesByDate(configuration, register, endDate, dimensions, valueProperties, valueAggregationTypes, _filterConverter.ConvertToInternal(filter));
        }

        public IEnumerable<dynamic> GetValuesBetweenDates(string configuration,
                                                          string register,
                                                          DateTime startDate,
                                                          DateTime endDate,
                                                          IEnumerable<string> dimensions = null,
                                                          IEnumerable<string> valueProperties = null,
                                                          IEnumerable<AggregationType> valueAggregationTypes = null,
                                                          Action<FilterBuilder> filter = null)
        {
            return _registerApi.GetValuesBetweenDates(configuration, register, startDate, endDate, dimensions, valueProperties, valueAggregationTypes, _filterConverter.ConvertToInternal(filter));
        }

        public IEnumerable<dynamic> GetRegisterEntries(string configuration,
                                                       string register,
                                                       Action<FilterBuilder> filter,
                                                       int pageNumber,
                                                       int pageSize)
        {
            return _registerApi.GetRegisterEntries(configuration, register, _filterConverter.ConvertToInternal(filter), pageNumber, pageSize);
        }
    }
}