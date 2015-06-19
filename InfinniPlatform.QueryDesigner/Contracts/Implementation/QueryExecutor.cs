using System.Collections.Generic;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.QueryDesigner.Contracts.Implementation
{
    public sealed class QueryExecutor : IQueryExecutor
    {
        public IEnumerable<dynamic> ExecuteQuery(string version, string queryText, bool denormalizeResult = false)
        {
            return new DocumentApi(version).GetDocumentByQuery(queryText, denormalizeResult);
        }
    }
}