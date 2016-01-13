using System;
using System.Collections.Generic;

using InfinniPlatform.Sdk.Documents;

using Newtonsoft.Json;

namespace InfinniPlatform.Sdk.RestApi
{
    public static class RequestExecutorExtensions
    {
        public static string CreateQueryString(IEnumerable<FilterCriteria> filter, int pageNumber, int pageSize, IEnumerable<SortingCriteria> sorting)
        {
            var jsonFilterStrings = JsonConvert.SerializeObject(filter);
            var jsonSortingStrings = JsonConvert.SerializeObject(sorting);

            return $"filter={Uri.EscapeDataString(jsonFilterStrings)}&pageNumber={pageNumber}&pageSize={pageSize}&sorting={Uri.EscapeDataString(jsonSortingStrings)}";
        }

        public static string CreateQueryStringCount(IEnumerable<FilterCriteria> filter)
        {
            var jsonFilterStrings = JsonConvert.SerializeObject(filter);

            return $"$filter={Uri.EscapeUriString(jsonFilterStrings)}";
        }
    }
}