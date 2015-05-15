using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Print;

namespace InfinniPlatform.FastReport.Templates.Reports
{
	/// <summary>
	/// Шаблон отчета.
	/// </summary>
	public sealed class ReportTemplate
	{
		/// <summary>
		/// Информация об отчете.
		/// </summary>
		public ReportInfo Info { get; set; }


		/// <summary>
		/// Параметры отчета.
		/// </summary>
		public ICollection<ParameterInfo> Parameters { get; set; }

		/// <summary>
		/// Источники данных.
		/// </summary>
		public ICollection<DataSourceInfo> DataSources { get; set; }

		/// <summary>
		/// Итоги отчета.
		/// </summary>
		public ICollection<TotalInfo> Totals { get; set; }


		/// <summary>
		/// Настройки печати.
		/// </summary>
		public PrintSetup PrintSetup { get; set; }


		/// <summary>
		/// Заголовок отчета.
		/// </summary>
		public ReportBand Title { get; set; }

		/// <summary>
		/// Страница отчета.
		/// </summary>
		public ReportPageBand Page { get; set; }

		/// <summary>
		/// Блок данных отчета.
		/// </summary>
		public ReportDataBand Data { get; set; }

		/// <summary>
		/// Итоги отчета.
		/// </summary>
		public ReportBand Summary { get; set; }
	}
}