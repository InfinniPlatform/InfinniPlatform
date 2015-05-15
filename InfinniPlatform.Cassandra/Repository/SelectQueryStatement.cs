namespace InfinniPlatform.Cassandra.Repository
{
	/// <summary>
	/// Represents query statement for selecting data.
	/// </summary>
	public sealed class SelectQueryStatement : IQueryStatement
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
		/// Columns to select.
		/// </summary>
		public string[] Columns;

		/// <summary>
		/// Filtering rules.
		/// </summary>
		public KeyFilter[] Where;

		/// <summary>
		/// Sorting rules.
		/// </summary>
		public KeySort[] Order;
	}
}