using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.SearchOptions.Converters;
using InfinniPlatform.Sdk;
using InfinniPlatform.Sdk.ApiContracts;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Api.RestApi.Public
{
    public class RegisterApi : IRegisterApi
    {
        public IEnumerable<dynamic> GetValuesByDate(string configuration, string register, DateTime endDate, IEnumerable<string> dimensions = null, IEnumerable<string> valueProperties = null, IEnumerable<AggregationType> valueAggregationTypes = null, Action<FilterBuilder> filter = null)
        {

            return new DataApi.RegisterApi().GetValuesByDate(configuration, register, endDate, dimensions,
                valueProperties, valueAggregationTypes, new FilterConverter().ConvertToInternal(filter));
        }

        public IEnumerable<dynamic> GetValuesBetweenDates(string configuration, string register, DateTime startDate, DateTime endDate, IEnumerable<string> dimensions = null, IEnumerable<string> valueProperties = null, IEnumerable<AggregationType> valueAggregationTypes = null, Action<FilterBuilder> filter = null)
        {
            return new DataApi.RegisterApi().GetValuesBetweenDates(configuration, register, startDate, endDate,
                dimensions, valueProperties, valueAggregationTypes, new FilterConverter().ConvertToInternal(filter));
        }

        public IEnumerable<dynamic> GetRegisterEntries(string configuration, string register, Action<FilterBuilder> filter, int pageNumber, int pageSize)
        {
            return new DataApi.RegisterApi().GetRegisterEntries(configuration, register, new FilterConverter().ConvertToInternal(filter), pageNumber, pageSize);
        }
    }
}
