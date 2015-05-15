namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Страница отчета.
	/// </summary>
	public sealed class ReportPageBand
	{
		/// <summary>
		/// Заголовок блока.
		/// </summary>
		public ReportBand Header { get; set; }

		/// <summary>
		/// Итоги блока.
		/// </summary>
		public ReportBand Footer { get; set; }
	}
}