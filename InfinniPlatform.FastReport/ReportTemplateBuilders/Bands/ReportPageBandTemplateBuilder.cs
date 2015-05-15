using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Bands
{
	sealed class ReportPageBandTemplateBuilder : IReportObjectTemplateBuilder<ReportPageBand>
	{
		public ReportPageBand BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var reportPage = (ReportPage)reportObject;

			return new ReportPageBand
					   {
						   Header = context.BuildTemplate<ReportBand>(reportPage.PageHeader),
						   Footer = context.BuildTemplate<ReportBand>(reportPage.PageFooter)
					   };
		}
	}
}