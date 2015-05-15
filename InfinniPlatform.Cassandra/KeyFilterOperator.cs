namespace InfinniPlatform.Cassandra
{
	/// <summary>
	/// Represents filter operator.
	/// </summary>
	public enum KeyFilterOperator
	{
		/// <summary>
		/// Represents an equality comparison.
		/// </summary>
		Equal,

		/// <summary>
		/// Represents a "less than" comparison.
		/// </summary>
		Less,

		/// <summary>
		/// Represents a "less than or equal" comparison.
		/// </summary>
		LessOrEqual,

		/// <summary>
		/// Represents a "greater than" comparison.
		/// </summary>
		Greater,

		/// <summary>
		/// Represents a "greater than or equal" comparison.
		/// </summary>
		GreaterOrEqual,
	}
}