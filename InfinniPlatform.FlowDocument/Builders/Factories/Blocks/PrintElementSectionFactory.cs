using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
	sealed class PrintElementSectionFactory : IPrintElementFactory
	{
		public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
		{
			var element = new Section
						  {
							  Margin = BuildHelper.DefaultMargin,
							  Padding = BuildHelper.DefaultPadding
						  };

			BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyTextProperties(element, elementMetadata);

			BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
			BuildHelper.ApplyBlockProperties(element, elementMetadata);

			// Генерация содержимого элемента

			var contentContext = CreateContentContext(element, buildContext);
			var blocks = buildContext.ElementBuilder.BuildElements(contentContext, elementMetadata.Blocks);

			if (blocks != null)
			{
				element.Blocks.AddRange(blocks);
			}

			BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
			BuildHelper.PostApplyTextProperties(element, elementMetadata);

			return element;
		}

		private static PrintElementBuildContext CreateContentContext(Section element, PrintElementBuildContext buildContext)
		{
			var contentWidth = BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding, element.BorderThickness);
			return buildContext.Create(contentWidth);
		}
	}
}