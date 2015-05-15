namespace InfinniPlatform.FastReport.Templates.Reports
{
	/// <summary>
	/// Информация об отчете.
	/// </summary>
	public sealed class ReportInfo
	{
		/// <summary>
		/// Наименование отчета.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Заголовок отчета.
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Описание отчета.
		/// </summary>
		public string Description { get; set; }
	}
}