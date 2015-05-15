using FastReport;
using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Elements
{
	sealed class ElementLayoutBuilder : IReportObjectBuilder<ElementLayout>
	{
		public void BuildObject(IReportObjectBuilderContext context, ElementLayout template, object parent)
		{
			var element = (ReportComponentBase)parent;
			element.Top = template.Top * Units.Millimeters;
			element.Left = template.Left * Units.Millimeters;

			// Размеры таблицы выставляются только в том случае, если они указаны явно,
			// иначе размер таблицы вычисляется автоматически

			if (!(element is TableObject) || template.Width > 0)
			{
				element.Width = template.Width * Units.Millimeters;
			}

			if (!(element is TableObject) || template.Height > 0)
			{
				element.Height = template.Height * Units.Millimeters;
			}
		}
	}
}