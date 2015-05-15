namespace InfinniPlatform.FastReport.Templates.Print
{
	/// <summary>
	/// Настройки печати отчета.
	/// </summary>
	public sealed class PrintSetup
	{
		/// <summary>
		/// Формат листа бумаги.
		/// </summary>
		public PrintPaper Paper { get; set; }

		/// <summary>
		/// Отступы на листе при печати.
		/// </summary>
		public PrintPaperMargin Margin { get; set; }

		/// <summary>
		/// Настройки принтера.
		/// </summary>
		public PrinterSettings Printer { get; set; }
	}
}