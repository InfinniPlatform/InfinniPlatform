namespace InfinniPlatform.FastReport.Templates.Print
{
	/// <summary>
	/// Настройки принтера.
	/// </summary>
	public sealed class PrinterSettings
	{
		/// <summary>
		/// Режим печати.
		/// </summary>
		public PrintMode PrintMode { get; set; }

		/// <summary>
		/// Ширина (меньшая сторона) листа для печати.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float PaperWidth { get; set; }

		/// <summary>
		/// Высота (большая сторона) листа для печати.
		/// </summary>
		/// <remarks>Измеряется в миллиметрах.</remarks>
		public float PaperHeight { get; set; }
	}
}