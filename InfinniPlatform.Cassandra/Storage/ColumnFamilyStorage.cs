using System;

namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents service to manage the key-value storage.
	/// </summary>
	sealed class ColumnFamilyStorage : IColumnFamilyStorage
	{
		public ColumnFamilyStorage(IQueryExecutor queryExecutor)
		{
			if (queryExecutor == null)
			{
				throw new ArgumentNullException();
			}

			_queryExecutor = queryExecutor;
		}


		private readonly IQueryExecutor _queryExecutor;


		/// <summary>
		/// Creates a new keyspace.
		/// </summary>
		/// <param name="statement">Query statement for creating keyspace.</param>
		public void CreateKeyspace(CreateKeyspaceQueryStatement statement)
		{
			_queryExecutor.Execute(statement);
		}

		/// <summary>
		/// Removes the keyspace.
		/// </summary>
		/// <param name="statement">Query statement for dropping keyspace.</param>
		public void DeleteKeyspace(DropKeyspaceQueryStatement statement)
		{
			_queryExecutor.Execute(statement);
		}

		/// <summary>
		/// Creates a new table.
		/// </summary>
		/// <param name="statement">Query statement for creating table.</param>
		public void CreateTable(CreateTableQueryStatement statement)
		{
			_queryExecutor.Execute(statement);
		}

		/// <summary>
		/// Removes the table.
		/// </summary>
		/// <param name="statement">Query statement for dropping table.</param>
		public void DeleteTable(DropTableQueryStatement statement)
		{
			_queryExecutor.Execute(statement);
		}
	}
}