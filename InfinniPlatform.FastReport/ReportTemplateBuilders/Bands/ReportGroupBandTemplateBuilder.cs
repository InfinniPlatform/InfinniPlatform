using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Bands
{
	sealed class ReportGroupBandTemplateBuilder : IReportObjectTemplateBuilder<ReportGroupBand>
	{
		public ReportGroupBand BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var groupHeaderBand = reportObject as GroupHeaderBand;

			return (groupHeaderBand != null)
					   ? new ReportGroupBand
							 {
								 DataBind = context.BuildTemplate<IDataBind>(groupHeaderBand.Condition),
								 Header = context.BuildTemplate<ReportBand>(groupHeaderBand),
								 Footer = context.BuildTemplate<ReportBand>(groupHeaderBand.GroupFooter)
							 }
					   : null;
		}
	}
}