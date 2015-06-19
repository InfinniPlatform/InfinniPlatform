using System.Windows.Documents;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
    internal sealed class PrintElementParagraphFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new Paragraph
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
                element.Inlines.AddRange(inlines);
            }

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }

        private static void ApplyIndent(Paragraph element, dynamic elementMetadata)
        {
            double indentSize;

            if (BuildHelper.TryToSizeInPixels(elementMetadata.IndentSize, elementMetadata.IndentSizeUnit, out indentSize))
            {
                element.TextIndent = indentSize;
            }
        }

        private static PrintElementBuildContext CreateContentContext(Paragraph element,
            PrintElementBuildContext buildContext)
        {
            var contentWidth = BuildHelper.CalcContentWidth(buildContext.ElementWidth, element.Margin, element.Padding,
                element.BorderThickness);
            return buildContext.Create(contentWidth);
        }
    }
}