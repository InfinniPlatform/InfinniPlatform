using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.RestApi.DataApi;

namespace InfinniPlatform.QueryDesigner.Contracts.Implementation
{
	public sealed class QueryExecutor : IQueryExecutor
	{
		public IEnumerable<dynamic> ExecuteQuery(string queryText, bool denormalizeResult = false)
		{
			return new DocumentApi().GetDocumentByQuery(queryText, denormalizeResult);
		}
	}
}
