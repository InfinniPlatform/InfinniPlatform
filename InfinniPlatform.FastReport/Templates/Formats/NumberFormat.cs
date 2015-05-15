namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Формат отображения числового значения.
	/// </summary>
	public class NumberFormat : IFormat
	{
		/// <summary>
		/// Количество знаков после запятой.
		/// </summary>
		public int DecimalDigits { get; set; }

		/// <summary>
		/// Разделитель между группами.
		/// </summary>
		public string GroupSeparator { get; set; }

		/// <summary>
		/// Разделитель между целой и дробной частью.
		/// </summary>
		public string DecimalSeparator { get; set; }
	}
}