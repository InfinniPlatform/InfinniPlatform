using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintHyperlinkFactory : PrintElementFactoryBase<PrintHyperlink>
    {
        public override object Create(PrintElementFactoryContext context, PrintHyperlink template)
        {
            var element = new PrintHyperlink();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            element.Reference = FactoryHelper.FormatValue(context, template.Reference, template.SourceFormat);

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