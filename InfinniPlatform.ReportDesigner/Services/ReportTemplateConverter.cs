using FastReport;

using InfinniPlatform.FastReport.ReportObjectBuilders;
using InfinniPlatform.FastReport.ReportTemplateBuilders;
using InfinniPlatform.FastReport.Templates.Reports;

namespace InfinniPlatform.ReportDesigner.Services
{
	sealed class ReportTemplateConverter
	{
		private readonly FrReportTemplateBuilder _templateBuilder = new FrReportTemplateBuilder();

		public ReportTemplate ConvertToTemplate(Report report)
		{
			var template = _templateBuilder.Build(report);

			return template;
		}


		private readonly FrReportBuilder _reportBuilder = new FrReportBuilder();

		public Report ConvertFromTemplate(ReportTemplate template)
		{
			var report = _reportBuilder.Build(template ?? new ReportTemplate());

			return (Report)report.Object;
		}
	}
}