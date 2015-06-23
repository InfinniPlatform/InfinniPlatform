using System.Collections.Generic;

namespace InfinniPlatform.Sdk
{
    public static class RequestExecutorExtensions
    {
        public static string CreateQueryString(IEnumerable<string> filter, int pageNumber, int pageSize, IEnumerable<string> sorting)
        {
            return string.Format("filter={0}&pageNumber={1}&pageSize={2}&sorting={3}",string.Join(" and ", filter), pageNumber, pageSize, string.Join(" and ", sorting));
        }
    }
}
