namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Типы данных.
	/// </summary>
	/// <remarks>
	/// За основу была взята спецификация "JSON Schema".
	/// </remarks>
	public enum DataType
	{
		/// <summary>
		/// Не определен.
		/// </summary>
		None = 0,

		/// <summary>
		/// Строка.
		/// </summary>
		String = 1,

		/// <summary>
		/// Дробное число.
		/// </summary>
		Float = 2,

		/// <summary>
		/// Целое число.
		/// </summary>
		Integer = 4,

		/// <summary>
		/// Логическое значение. 
		/// </summary>
		Boolean = 8,

		/// <summary>
		/// Дата/время.
		/// </summary>
		DateTime = 16,

		/// <summary>
		/// Объект.
		/// </summary>
		Object = 32,

		/// <summary>
		/// Массив.
		/// </summary>
		Array = 64,
	}
}