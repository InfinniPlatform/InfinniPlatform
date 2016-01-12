using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.Environment.Index;
using InfinniPlatform.Sdk.RestApi;

namespace InfinniPlatform.Core.RestApi.Public
{
    public class RegisterApi : IRegisterApi
    {
        public RegisterApi(DataApi.RegisterApi registerApi)
        {
            _registerApi = registerApi;
        }


        private readonly DataApi.RegisterApi _registerApi;
        

        public IEnumerable<dynamic> GetValuesByDate(string configuration,
                                                    string register,
                                                    DateTime endDate,
                                                    IEnumerable<string> dimensions = null,
                                                    IEnumerable<string> valueProperties = null,
                                                    IEnumerable<AggregationType> valueAggregationTypes = null,
                                                    Action<FilterBuilder> filter = null)
        {
            return _registerApi.GetValuesByDate(configuration, register, endDate, dimensions, valueProperties, valueAggregationTypes, filter);
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
            return _registerApi.GetValuesBetweenDates(configuration, register, startDate, endDate, dimensions, valueProperties, valueAggregationTypes, filter);
        }

        public IEnumerable<dynamic> GetRegisterEntries(string configuration,
                                                       string register,
                                                       Action<FilterBuilder> filter,
                                                       int pageNumber,
                                                       int pageSize)
        {
            return _registerApi.GetRegisterEntries(configuration, register, filter, pageNumber, pageSize);
        }
    }
}