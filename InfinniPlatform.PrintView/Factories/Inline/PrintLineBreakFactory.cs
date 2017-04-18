using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintLineBreakFactory : PrintElementFactoryBase<PrintLineBreak>
    {
        public override object Create(PrintElementFactoryContext context, PrintLineBreak template)
        {
            var element = new PrintLineBreak();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            return element;
        }
    }
}