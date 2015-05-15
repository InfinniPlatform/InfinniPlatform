namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents sorting rule.
	/// </summary>
	public sealed class KeySort
	{
		public KeySort(string sortKey, KeySortDirection sortDirection = KeySortDirection.Asc)
		{
			Key = sortKey;
			Direction = sortDirection;
		}

		/// <summary>
		/// Sort key.
		/// </summary>
		public readonly string Key;

		/// <summary>
		/// Sort direction.
		/// </summary>
		public readonly KeySortDirection Direction;
	}
}