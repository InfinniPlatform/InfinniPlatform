using System;
using System.Collections.Generic;
using System.Linq;

namespace InfinniPlatform.Cassandra.Repository
{
	/// <summary>
	/// Represents data repository for the key-value storage.
	/// </summary>
	sealed class ColumnFamilyRepository : IColumnFamilyRepository
	{
		public ColumnFamilyRepository(IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null)
			{
				throw new ArgumentNullException();
			}

			_queryExecutor = queryExecutor;
		}


		private readonly IQueryExecutor _queryExecutor;


		/// <summary>
		/// Returns number of rows for the specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public int Count(CountQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One)
		{
			var rows = _queryExecutor.Execute(statement, parameters, consistency);

			if (rows != null)
			{
				var first = rows.FirstOrDefault();

				if (first != null)
				{
					return first.GetValue<int>(0);
				}
			}

			return 0;
		}

		/// <summary>
		/// Reurns rows for the specified query.
		/// </summary>
		/// <param name="statement"></param>
		/// <param name="parameters"></param>
		/// <param name="consistency"></param>
		public IEnumerable<IRow> Fetch(SelectQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One)
		{
			return _queryExecutor.Execute(statement, parameters, consistency);
		}
	}
}