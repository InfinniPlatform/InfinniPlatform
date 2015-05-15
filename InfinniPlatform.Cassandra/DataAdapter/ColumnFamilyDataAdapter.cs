using System;

namespace InfinniPlatform.Cassandra.DataAdapter
{
	/// <summary>
	/// Represents data adapter for the key-value storage.
	/// </summary>
	sealed class ColumnFamilyDataAdapter : IColumnFamilyDataAdapter
	{
		public ColumnFamilyDataAdapter(IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null)
			{
				throw new ArgumentNullException();
			}

			_queryExecutor = queryExecutor;
		}


		private readonly IQueryExecutor _queryExecutor;


		/// <summary>
		/// Executes insert data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public void Insert(InsertQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One)
		{
			_queryExecutor.Execute(statement, parameters, consistency);
		}

		/// <summary>
		/// Executes update data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public void Update(UpdateQueryStatement statement, object[] parameters, Consistency consistency = Consistency.One)
		{
			_queryExecutor.Execute(statement, parameters, consistency);
		}

		/// <summary>
		/// Executes delete data.
		/// </summary>
		/// <param name="statement">Query statement.</param>
		/// <param name="parameters">Query parameters.</param>
		/// <param name="consistency">Consistency level.</param>
		public void Delete(DeleteQueryStatement statement, object[] parameters = null, Consistency consistency = Consistency.One)
		{
			_queryExecutor.Execute(statement, parameters, consistency);
		}
	}
}