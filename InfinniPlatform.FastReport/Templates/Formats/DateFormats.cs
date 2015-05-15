namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Формат отображения даты.
	/// </summary>
	public enum DateFormats
	{
		/// <summary>
		/// По умолчанию
		/// </summary>
		Default = 0,

		/// <summary>
		/// Короткое строковое представление даты (например, 01.01.2011).
		/// </summary>
		ShortDate = 1,

		/// <summary>
		/// Длинное строковое представление даты (например, 1 января 2011 г.).
		/// </summary>
		LongDate = 2,
	}
}