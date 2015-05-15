namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents query statement for creating keyspace.
	/// </summary>
	public sealed class CreateKeyspaceQueryStatement : IQueryStatement
	{
		/// <summary>
		/// Keyspace name.
		/// </summary>
		public string Keyspace;
	}
}