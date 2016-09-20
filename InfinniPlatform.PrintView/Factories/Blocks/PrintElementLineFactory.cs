using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Factories.Blocks
{
    internal class PrintElementLineFactory : IPrintElementFactory
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