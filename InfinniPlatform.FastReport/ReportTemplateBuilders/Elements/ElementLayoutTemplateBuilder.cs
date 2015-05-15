using FastReport;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Elements
{
	sealed class ElementLayoutTemplateBuilder : IReportObjectTemplateBuilder<ElementLayout>
	{
		public ElementLayout BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var element = (ReportComponentBase)reportObject;

			return new ElementLayout
					   {
						   Top = element.Top / Units.Millimeters,
						   Left = element.Left / Units.Millimeters,
						   Width = element.Width / Units.Millimeters,
						   Height = element.Height / Units.Millimeters
					   };
		}
	}
}