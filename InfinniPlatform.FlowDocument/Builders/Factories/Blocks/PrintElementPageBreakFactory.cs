using System.Windows.Documents;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
    internal sealed class PrintElementPageBreakFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new Paragraph
            {
                FontSize = 0.1,
                Margin = BuildHelper.DefaultMargin,
                Padding = BuildHelper.DefaultPadding,
                BreakPageBefore = true
            };

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyBlockProperties(element, elementMetadata);

            return element;
        }
    }
}