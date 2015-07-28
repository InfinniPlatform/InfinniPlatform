using InfinniPlatform.FlowDocument.Model.Inlines;
//using System.Windows.Documents;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Inlines
{
	sealed class PrintElementUnderlineFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new Underline();

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyInlineProperties(element, elementMetadata);

			// Генерация содержимого элемента

			var inlines = buildContext.ElementBuilder.BuildElements(buildContext, elementMetadata.Inlines);

			if (inlines != null)
			{
				element.Inlines.AddRange(inlines);
			}

			BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.PostApplyTextProperties(element, elementMetadata);

			return element;
		}
	}
}