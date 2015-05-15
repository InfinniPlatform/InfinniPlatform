using System.Collections.Generic;

namespace InfinniPlatform.Cassandra.Repository
{
	/// <summary>
	/// Represents data repository for the key-value storage.
	/// </summary>
	public interface IColumnFamilyRepository
	{
		/// <summary>
		/// Returns number of rows for the specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		int Count(CountQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One);

		/// <summary>
		/// Reurns rows for the specified query.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		IEnumerable<IRow> Fetch(SelectQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One);
	}
}