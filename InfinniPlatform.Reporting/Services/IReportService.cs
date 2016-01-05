using System.Collections.Generic;

using InfinniPlatform.Core.Reporting;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.Reporting.Services
{
	/// <summary>
	/// Сервис для работы с подсистемой отчетов.
	/// </summary>
	public interface IReportService
	{
		/// <summary>
		/// Получить значения параметра отчета.
		/// </summary>
		/// <param name="template">Шаблон отчета.</param>
		/// <returns>Значения параметров отчета.</returns>
		IDictionary<string, ParameterValues> GetParameterValues(ReportTemplate template);

		/// <summary>
		/// Создать файл отчета.
		/// </summary>
		/// <param name="template">Шаблон отчета.</param>
		/// <param name="parameterValues">Значения параметров отчета.</param>
		/// <param name="fileFormat">Формат файла отчета.</param>
		/// <returns>Файл отчета.</returns>
		byte[] CreateReportFile(ReportTemplate template, IDictionary<string, object> parameterValues = null, ReportFileFormat fileFormat = ReportFileFormat.Pdf);
	}
}