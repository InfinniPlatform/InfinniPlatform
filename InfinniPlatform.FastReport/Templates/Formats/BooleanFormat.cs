namespace InfinniPlatform.FastReport.Templates.Formats
{
	/// <summary>
	/// Формат отображения логического значения.
	/// </summary>
	public sealed class BooleanFormat : IFormat
	{
		/// <summary>
		/// Текст для отображения ложного значения.
		/// </summary>
		public string FalseText { get; set; }

		/// <summary>
		/// Текст для отображения истинного значения.
		/// </summary>
		public string TrueText { get; set; }
	}
}