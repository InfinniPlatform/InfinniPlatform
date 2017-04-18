using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintSpanFactory : PrintElementFactoryBase<PrintSpan>
    {
        public override object Create(PrintElementFactoryContext context, PrintSpan template)
        {
            var element = new PrintSpan();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            // Создание содержимого элемента

            var inlines = context.Factory.BuildElements(context, template.Inlines);

            if (inlines != null)
            {
                foreach (PrintInline inline in inlines)
                {
                    element.Inlines.Add(inline);
                }
            }

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }
    }
}