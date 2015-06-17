using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.QueryDesigner.Contracts
{
	public interface IQueryExecutor
	{
        IEnumerable<dynamic> ExecuteQuery(string version, string queryText, bool denormalizeResult = false);
	}
}
