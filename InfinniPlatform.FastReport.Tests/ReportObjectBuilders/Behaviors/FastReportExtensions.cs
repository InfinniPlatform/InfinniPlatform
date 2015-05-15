using System.Collections.Generic;
using System.Linq;

using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Reports;

using FRReport = FastReport.Report;

namespace InfinniPlatform.FastReport.Tests.ReportObjectBuilders.Behaviors
{
	static class FastReportExtensions
	{
		/// <summary>
		/// Сформировать отчет 
		/// </summary>
		/// <param name="template">Шаблон отчета</param>
		/// <returns>Отчет Fast Report</returns>
		public static FRReport BuildFastReport(this ReportTemplate template)
		{
			var buider = new FrReportBuilder();
			return (FRReport)buider.Build(template).Object;
		}

		/// <summary>
		/// Получить первую страницу отчета
		/// </summary>
		/// <param name="report">Отчет Fast Report</param>
		public static ReportPage FirstPage(this FRReport report)
		{
			return (report != null && report.Pages.Count > 0) ? (ReportPage)report.Pages[0] : null;
		}

		/// <summary>
		/// Найти дочерний элемент заданного типа 
		/// </summary>
		/// <typeparam name="TElement">Тип дочернего элемента</typeparam>
		/// <param name="parent">Родительский элемент</param>
		/// <returns>Первый из найденных дочерних элементов <paramref name="parent"/> типа <typeparamref name="TElement"/></returns>
		public static TElement FindChildElement<TElement>(this Base parent) where TElement : Base
		{
			return (parent != null && parent.ChildObjects != null)
						? (TElement)parent.ChildObjects.Cast<Base>().FirstOrDefault(i => i is TElement)
						: null;
		}

		public static TotalInfo Resolve(this ICollection<TotalInfo> target, string name)
		{
			return target.FirstOrDefault(i => i.Name == name);
		}
	}
}