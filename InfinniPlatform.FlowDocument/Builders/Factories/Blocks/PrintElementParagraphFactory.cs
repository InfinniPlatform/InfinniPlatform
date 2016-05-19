using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
	sealed class PrintElementParagraphFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new PrintElementParagraph
						  {
							  Margin = BuildHelper.DefaultMargin,
							  Padding = BuildHelper.DefaultPadding
						  };

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyBlockProperties(element, elementMetadata);

			ApplyIndent(element, elementMetadata);

			// Генерация содержимого элемента

			var contentContext = CreateContentContext(element, buildContext);
			var inlines = buildContext.ElementBuilder.BuildElements(contentContext, elementMetadata.Inlines);

			if (inlines != null)
			{
			    foreach (var inline in inlines)
			    {
			        element.Inlines.Add(inline);
			    }
			}

			BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.PostApplyTextProperties(element, elementMetadata);

			return element;
		}

		private static void ApplyIndent(PrintElementParagraph element, dynamic elementMetadata)
		{
			double indentSize;

			if (BuildHelper.TryToSizeInPixels(elementMetadata.IndentSize, elementMetadata.IndentSizeUnit, out indentSize))
			{
				element.IndentSize = indentSize;
			}
		}

		private static PrintElementBuildContext CreateContentContext(PrintElementParagraph element, PrintElementBuildContext buildContext)
		{
		    var contentWidth = (element.Border != null)
		        ? BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding, element.Border.Thickness)
		        : BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding);

			return buildContext.Create(contentWidth);
		}
	}
}