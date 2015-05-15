namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents service to manage the key-value storage.
	/// </summary>
	public interface IColumnFamilyStorage
	{
		/// <summary>
		/// Creates a new keyspace.
		/// </summary>
		/// <param name="statement">Query statement for creating keyspace.</param>
		void CreateKeyspace(CreateKeyspaceQueryStatement statement);

		/// <summary>
		/// Removes the keyspace.
		/// </summary>
		/// <param name="statement">Query statement for dropping keyspace.</param>
		void DeleteKeyspace(DropKeyspaceQueryStatement statement);

		/// <summary>
		/// Creates a new table.
		/// </summary>
		/// <param name="statement">Query statement for creating table.</param>
		void CreateTable(CreateTableQueryStatement statement);

		/// <summary>
		/// Removes the table.
		/// </summary>
		/// <param name="statement">Query statement for dropping table.</param>
		void DeleteTable(DropTableQueryStatement statement);
	}
}