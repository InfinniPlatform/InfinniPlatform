using System.Collections.Generic;

namespace InfinniPlatform.QueryDesigner.Contracts
{
    public interface IQueryExecutor
    {
        IEnumerable<dynamic> ExecuteQuery(string version, string queryText, bool denormalizeResult = false);
    }
}