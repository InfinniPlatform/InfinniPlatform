using FastReport;

using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Reports
{
	sealed class ReportObjectBuilder : IReportObjectBuilder<ReportTemplate>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportTemplate template, object parent)
		{
			// Создание шаблона отчета

			var reportInfo = template.Info ?? new Templates.Reports.ReportInfo();

			var report = new Report
						 {
							 ReportInfo =
							 {
								 Name = reportInfo.Name ?? string.Empty,
								 Description = reportInfo.Description ?? string.Empty
							 }
						 };

			var reportPage = context.CreateObject<ReportPage>();
			report.Pages.Add(reportPage);

			// Регистрация параметров отчета
			context.BuildObjects(template.Parameters, report);

			// Регистрация источников данных отчета
			context.BuildObjects(template.DataSources, report);

			// Настройка принтера и правил печати отчета
			context.BuildObject(template.PrintSetup, report);

			// Построение и настройка блоков отчета

			if (template.Title != null)
			{
				reportPage.ReportTitle = context.CreateObject<ReportTitleBand>();
				context.BuildObject(template.Title, reportPage.ReportTitle);
			}

			if (template.Summary != null)
			{
				reportPage.ReportSummary = context.CreateObject<ReportSummaryBand>();
				context.BuildObject(template.Summary, reportPage.ReportSummary);
			}

			context.BuildObject(template.Page, reportPage);
			context.BuildObject(template.Data, reportPage);

			// Регистрация итогов отчета
			context.BuildObjects(template.Totals, report);

			// Регистрация результатов построения
			context.Report = report;
		}
	}
}