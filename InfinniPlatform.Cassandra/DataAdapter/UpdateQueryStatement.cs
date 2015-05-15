namespace InfinniPlatform.Cassandra.DataAdapter
{
	/// <summary>
	/// Represents query statement for updating data.
	/// </summary>
	public sealed class UpdateQueryStatement : IQueryStatement
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
		/// Columns to update.
		/// </summary>
		public string[] Columns;

		/// <summary>
		/// Filtering rules.
		/// </summary>
		public KeyFilter[] Where;
	}
}