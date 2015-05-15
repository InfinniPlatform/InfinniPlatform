namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Формат отображения времени.
	/// </summary>
	public enum TimeFormats
	{
		/// <summary>
		/// По умолчанию
		/// </summary>
		Default = 0,

		/// <summary>
		/// Короткое строковое представление времени (например, 12:34).
		/// </summary>
		ShortTime = 1,

		/// <summary>
		/// Длинное строковое представление времени (например, 12:34:56).
		/// </summary>
		LongTime = 2,
	}
}