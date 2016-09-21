using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Factories.Blocks
{
    internal class PrintElementSectionFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementSection
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
                foreach (var block in blocks)
                {
                    element.Blocks.Add(block);
                }
            }

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }

        private static PrintElementBuildContext CreateContentContext(PrintElementSection element, PrintElementBuildContext buildContext)
        {
            var contentWidth = (element.Border != null)
                ? BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding, element.Border.Thickness)
                : BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding);
            return buildContext.Create(contentWidth);
        }
    }
}