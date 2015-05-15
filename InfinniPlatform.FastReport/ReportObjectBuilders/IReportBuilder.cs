using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.ReportObjectBuilders
{
	/// <summary>
	/// Предоставляет методы для формирования отчета.
	/// </summary>
	public interface IReportBuilder
	{
		/// <summary>
		/// Построить отчет по шаблону.
		/// </summary>
		/// <param name="template">Шаблон отчета.</param>
		/// <returns>Объект отчета.</returns>
		IReport Build(ReportTemplate template);
	}
}