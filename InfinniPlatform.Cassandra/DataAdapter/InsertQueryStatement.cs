namespace InfinniPlatform.Cassandra.DataAdapter
{
	/// <summary>
	/// Represents query statement for inserting data.
	/// </summary>
	public sealed class InsertQueryStatement : IQueryStatement
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
		/// Columns to insert.
		/// </summary>
		public string[] Columns;
	}
}