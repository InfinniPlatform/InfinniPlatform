using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Bands
{
	sealed class ReportPageBandBuilder : IReportObjectBuilder<ReportPageBand>
	{
		public void BuildObject(IReportObjectBuilderContext context, ReportPageBand template, object parent)
		{
			var reportPage = (ReportPage)parent;

			// Построение блока заголовка страницы

			if (template.Header != null)
			{
				reportPage.PageHeader = new PageHeaderBand();
				context.BuildObject(template.Header, reportPage.PageHeader);
			}

			// Построение блока итогов страницы

			if (template.Footer != null)
			{
				reportPage.PageFooter = new PageFooterBand();
				context.BuildObject(template.Footer, reportPage.PageFooter);
			}
		}
	}
}