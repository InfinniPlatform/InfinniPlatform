using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Factories.Inlines
{
    internal class PrintElementRunFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementRun();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyInlineProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyInlineProperties(element, elementMetadata);

            ApplyText(element, buildContext, elementMetadata);

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }

        private static void ApplyText(PrintElementRun element, PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            element.Text = BuildHelper.FormatValue(buildContext, elementMetadata.Text, elementMetadata.SourceFormat);
        }
    }
}