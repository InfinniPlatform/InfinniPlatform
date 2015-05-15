namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents filtering rule.
	/// </summary>
	public sealed class KeyFilter
	{
		public KeyFilter(string filterKey, KeyFilterOperator filterOperator = KeyFilterOperator.Equal)
		{
			Key = filterKey;
			Operator = filterOperator;
		}

		/// <summary>
		/// Filter key.
		/// </summary>
		public readonly string Key;

		/// <summary>
		/// Filter operator.
		/// </summary>
		public readonly KeyFilterOperator Operator;
	}
}