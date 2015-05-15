namespace InfinniPlatform.FastReport.Templates.Bands
{
	/// <summary>
	/// Настройки печати блока отчета.
	/// </summary>
	public sealed class ReportBandPrintSetup
	{
		/// <summary>
		/// Печатать на новой странице.
		/// </summary>
		public bool IsStartNewPage { get; set; }

		/// <summary>
		/// Местоположение печати блока.
		/// </summary>
		public PrintTargets PrintTargets { get; set; }
	}
}