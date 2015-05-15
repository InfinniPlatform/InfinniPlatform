namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents query statement for dropping keyspace.
	/// </summary>
	public sealed class DropKeyspaceQueryStatement : IQueryStatement
	{
		/// <summary>
		/// Keyspace name.
		/// </summary>
		public string Keyspace;
	}
}