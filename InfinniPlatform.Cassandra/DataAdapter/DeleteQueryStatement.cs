namespace InfinniPlatform.Cassandra.DataAdapter
{
	/// <summary>
	/// Represents query statement for deleting data.
	/// </summary>
	public sealed class DeleteQueryStatement : IQueryStatement
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
		/// Columns to delete.
		/// </summary>
		public string[] Columns;

		/// <summary>
		/// Filtering rules.
		/// </summary>
		public KeyFilter[] Where;
	}
}