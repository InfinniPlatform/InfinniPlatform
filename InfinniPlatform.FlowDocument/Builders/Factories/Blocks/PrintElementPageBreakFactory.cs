using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Builders.Factories.Blocks
{
    sealed class PrintElementPageBreakFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementPageBreak();

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyBlockProperties(element, elementMetadata);

            return element;
        }
    }
}