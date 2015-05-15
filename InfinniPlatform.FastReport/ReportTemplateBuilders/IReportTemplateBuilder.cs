using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders
{
	/// <summary>
	/// Предоставляет методы для формирования шаблона отчета по объекту отчета.
	/// </summary>
	public interface IReportTemplateBuilder
	{
		/// <summary>
		/// Построить шаблон отчета.
		/// </summary>
		/// <param name="reportObject">Объект отчета.</param>
		/// <returns>Шаблон отчета.</returns>
		ReportTemplate Build(object reportObject);
	}
}