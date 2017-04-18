using InfinniPlatform.PrintView.Abstractions.Block;
using InfinniPlatform.PrintView.Abstractions.Defaults;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintLineFactory : PrintElementFactoryBase<PrintLine>
    {
        public override object Create(PrintElementFactoryContext context, PrintLine template)
        {
            var element = new PrintLine();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            element.Border = template.Border ?? context.ElementStyle?.Border ?? PrintViewDefaults.Line.Border;

            return element;
        }
    }
}