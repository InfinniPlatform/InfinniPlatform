using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Хранилище шаблонов отчетов.
	/// </summary>
	public interface IReportTemplateRepository
	{
		/// <summary>
		/// Получить шаблон отчета.
		/// </summary>
		/// <param name="templateId">Идентификатор отчета.</param>
		/// <returns>Шаблон отчета.</returns>
		ReportTemplate GetReportTemplate(string templateId);

		/// <summary>
		/// Сохранить шаблон отчета.
		/// </summary>
		/// <param name="templateId">Идентификатор отчета.</param>
		/// <param name="template">Шаблон отчета.</param>
		void SaveReportTemplate(string templateId, ReportTemplate template);
	}
}