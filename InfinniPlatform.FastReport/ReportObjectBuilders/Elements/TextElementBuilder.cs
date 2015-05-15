using FastReport;

using InfinniPlatform.FastReport.Templates.Elements;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Elements
{
	sealed class TextElementBuilder : IReportObjectBuilder<TextElement>
	{
		public void BuildObject(IReportObjectBuilderContext context, TextElement template, object parent)
		{
			var container = (IParent)parent;

			var textElement = context.CreateObject<TextObject>();
			textElement.CanGrow = template.CanGrow;
			textElement.CanShrink = template.CanShrink;
			container.AddChild(textElement);

			context.BuildObject(template.Border, textElement);
			context.BuildObject(template.Layout, textElement);
			context.BuildObject(template.Style, textElement);
			context.BuildObject(template.Format, textElement);
			context.BuildObject(template.DataBind, textElement);
		}
	}
}