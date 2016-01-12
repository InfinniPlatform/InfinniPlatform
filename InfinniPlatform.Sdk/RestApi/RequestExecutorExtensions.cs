using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.RestApi
{
    public static class RequestExecutorExtensions
    {
        public static string CreateQueryString(IEnumerable<FilterBuilder.CriteriaBuilder.CriteriaFilter> filter, int pageNumber, int pageSize, IEnumerable<CriteriaSorting> sorting)
        {
            var jsonFilterStrings = JsonConvert.SerializeObject(filter);
            var jsonSortingStrings = JsonConvert.SerializeObject(sorting);

            return $"filter={Uri.EscapeUriString(jsonFilterStrings)}&pageNumber={pageNumber}&pageSize={pageSize}&sorting={Uri.EscapeUriString(jsonSortingStrings)}";
        }

        public static string CreateQueryStringCount(IEnumerable<FilterBuilder.CriteriaBuilder.CriteriaFilter> filter)
        {
            var jsonFilterStrings = JsonConvert.SerializeObject(filter);

            return $"$filter={Uri.EscapeUriString(jsonFilterStrings)}";
        }
    }
}