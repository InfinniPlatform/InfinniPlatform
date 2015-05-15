namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents column.
	/// </summary>
	public sealed class Column
	{
		public Column(string name, ColumnType type, bool key = false)
		{
			Name = name;
			Type = type;
			Key = key;
		}

		/// <summary>
		/// Column is part of a primary key.
		/// </summary>
		public readonly bool Key;

		/// <summary>
		/// Column name.
		/// </summary>
		public readonly string Name;

		/// <summary>
		/// Column type.
		/// </summary>
		public readonly ColumnType Type;
	}
}