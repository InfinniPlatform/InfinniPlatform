using System.Collections.Generic;
using System.Linq;

using FastReport;

using InfinniPlatform.FastReport.Templates.Bands;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Bands
{
	sealed class ReportDataBandTemplateBuilder : IReportObjectTemplateBuilder<ReportDataBand>
	{
		public ReportDataBand BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var dataBand = (DataBand)reportObject;

			return new ReportDataBand
					   {
						   DataBind = context.BuildTemplate<CollectionBind>(dataBand),
						   Content = context.BuildTemplate<ReportBand>(dataBand),
						   Details = context.BuildTemplate<ReportDataBand>(dataBand.ChildObjects.OfType<DataBand>().FirstOrDefault()),
						   Groups = GetNestedGroups(context, dataBand.Parent)
					   };
		}

		private static ICollection<ReportGroupBand> GetNestedGroups(IReportObjectTemplateBuilderContext context, Base groupHeaderBand)
		{
			var groups = new Stack<ReportGroupBand>();

			while (groupHeaderBand is GroupHeaderBand)
			{
				var groupBand = context.BuildTemplate<ReportGroupBand>(groupHeaderBand);

				groups.Push(groupBand);

				groupHeaderBand = groupHeaderBand.Parent;
			}

			return groups.ToList();
		}
	}
}