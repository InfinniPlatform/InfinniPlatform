using System.Collections.Generic;

namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents query executor with specified lifetime policy for the session.
	/// </summary>
	interface IQueryExecutor
	{
		/// <summary>
		/// Executes specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		IEnumerable<IRow> Execute(IQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One);
	}
}