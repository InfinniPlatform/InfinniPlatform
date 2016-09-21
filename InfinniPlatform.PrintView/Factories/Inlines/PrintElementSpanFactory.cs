using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Factories.Inlines
{
    internal class PrintElementSpanFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementSpan();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            // Генерация содержимого элемента

            var inlines = buildContext.ElementBuilder.BuildElements(buildContext, elementMetadata.Inlines);

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
    }
}