namespace InfinniPlatform.Cassandra.Repository
{
	/// <summary>
	/// Represents query statement for counting data.
	/// </summary>
	public sealed class CountQueryStatement : IQueryStatement
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
		/// Number of returned rows.
		/// </summary>
		public int Limit;

		/// <summary>
		/// Filtering rules.
		/// </summary>
		public KeyFilter[] Where;
	}
}