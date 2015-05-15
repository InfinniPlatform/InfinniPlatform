using System.Linq;

using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Print;
using InfinniPlatform.FastReport.Templates.Reports;

using ReportInfo = InfinniPlatform.FastReport.Templates.Reports.ReportInfo;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Reports
{
	sealed class ReportTemplateBuilder : IReportObjectTemplateBuilder<ReportTemplate>
	{
		public ReportTemplate BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var report = (Report)reportObject;
			var reportPage = (ReportPage)report.Pages[0];

			var reportInfo = new ReportInfo
							 {
								 Name = report.ReportInfo.Name,
								 Description = report.ReportInfo.Description
							 };

			return new ReportTemplate
				   {
					   Info = reportInfo,
					   PrintSetup = context.BuildTemplate<PrintSetup>(report),
					   Title = context.BuildTemplate<ReportBand>(reportPage.ReportTitle),
					   Page = context.BuildTemplate<ReportPageBand>(reportPage),
					   Data = context.BuildTemplate<ReportDataBand>(reportPage.AllObjects.OfType<DataBand>().FirstOrDefault()),
					   Summary = context.BuildTemplate<ReportBand>(reportPage.ReportSummary)
				   };
		}
	}
}