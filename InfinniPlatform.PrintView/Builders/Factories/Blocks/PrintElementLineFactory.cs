using InfinniPlatform.FlowDocument.Model;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
    sealed class PrintElementLineFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementLine
            {
                Border = new PrintElementBorder
                {
                    Thickness = new PrintElementThickness(0, 0, 0, 1),
                    Color = "Black"
                }
            };

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyBlockProperties(element, elementMetadata);

            return element;
        }
    }
}