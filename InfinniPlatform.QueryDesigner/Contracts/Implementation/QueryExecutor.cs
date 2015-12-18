using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.QueryDesigner.Contracts.Implementation
{
    public sealed class QueryExecutor : IQueryExecutor
    {
        public IEnumerable<dynamic> ExecuteQuery(string version, string queryText, bool denormalizeResult = false)
        {
            return Enumerable.Empty<object>();
        }
    }
}