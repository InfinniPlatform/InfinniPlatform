using System;
using System.Collections.Generic;

namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents session.
	/// </summary>
	interface ISession : IDisposable
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