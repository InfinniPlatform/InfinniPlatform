using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Elements
{
	sealed class TextElementTemplateBuilder : IReportObjectTemplateBuilder<TextElement>
	{
		public TextElement BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var textObject = (global::FastReport.TextObject)reportObject;

			return new TextElement
					   {
						   Border = context.BuildTemplate<Border>(textObject.Border),
						   Layout = context.BuildTemplate<ElementLayout>(textObject),
						   Style = context.BuildTemplate<TextElementStyle>(textObject),
						   Format = context.BuildTemplate<IFormat>(textObject.Format),
						   DataBind = context.BuildTemplate<IDataBind>(textObject.Text),
						   CanGrow = textObject.CanGrow,
						   CanShrink = textObject.CanShrink
					   };
		}
	}
}