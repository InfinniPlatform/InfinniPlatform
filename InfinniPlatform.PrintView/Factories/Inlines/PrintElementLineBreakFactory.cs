using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Factories.Inlines
{
    internal class PrintElementLineBreakFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementLineBreak();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            return element;
        }
    }
}