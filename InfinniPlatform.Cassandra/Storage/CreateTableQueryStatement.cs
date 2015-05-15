namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents query statement for creating table.
	/// </summary>
	public sealed class CreateTableQueryStatement : IQueryStatement
	{
		/// <summary>
		/// Keyspace name.
		/// </summary>
		public string Keyspace;

		/// <summary>
		/// Table name.
		/// </summary>
		public string Table;

		/// <summary>
		/// Table columns.
		/// </summary>
		public Column[] Columns;
	}
}