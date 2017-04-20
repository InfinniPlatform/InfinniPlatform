using InfinniPlatform.PrintView.Block;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintPageBreakFactory : PrintElementFactoryBase<PrintPageBreak>
    {
        public override object Create(PrintElementFactoryContext context, PrintPageBreak template)
        {
            var element = new PrintPageBreak();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            return element;
        }
    }
}