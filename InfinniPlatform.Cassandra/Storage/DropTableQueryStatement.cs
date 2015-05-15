namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents query statement for dropping table.
	/// </summary>
	public sealed class DropTableQueryStatement : IQueryStatement
	{
		/// <summary>
		/// Keyspace name.
		/// </summary>
		public string Keyspace;

		/// <summary>
		/// Table name.
		/// </summary>
		public string Table;
	}
}