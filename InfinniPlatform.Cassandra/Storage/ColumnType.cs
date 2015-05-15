namespace InfinniPlatform.Cassandra.Storage
{
	/// <summary>
	/// Represents column type.
	/// </summary>
	public enum ColumnType
	{
		/// <summary>
		/// Represents a Boolean value.
		/// </summary>
		Bool,

		/// <summary>
		/// Represents a 32-bit signed integer.
		/// </summary>
		Int32,

		/// <summary>
		/// Represents a 64-bit signed integer.
		/// </summary>
		Int64,

		/// <summary>
		/// Represents a single-precision floating-point number.
		/// </summary>
		Float,

		/// <summary>
		/// Represents a double-precision floating-point number.
		/// </summary>
		Double,

		/// <summary>
		/// Represents a decimal number.
		/// </summary>
		Decimal,

		/// <summary>
		/// Represents a universally unique identifier (UUID).
		/// </summary>
		Uuid,

		/// <summary>
		/// Represents an instant in time, typically expressed as a date and time of day.
		/// </summary>
		DateTime,

		/// <summary>
		/// Represents text as a series of Unicode characters.
		/// </summary>
		String,

		/// <summary>
		/// Arbitrary bytes.
		/// </summary>
		Blob,
	}
}