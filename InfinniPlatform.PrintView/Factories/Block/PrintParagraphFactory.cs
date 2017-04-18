using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Abstractions.Block;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintParagraphFactory : PrintElementFactoryBase<PrintParagraph>
    {
        public override object Create(PrintElementFactoryContext context, PrintParagraph template)
        {
            var element = new PrintParagraph
            {
                IndentSize = template.IndentSize,
                IndentSizeUnit = template.IndentSizeUnit
            };

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            // Создание вложенных элементов

            var contentContext = context.CreateContentContext(element.Margin, element.Padding, element.Border?.Thickness);
            var inlines = context.Factory.BuildElements(contentContext, template.Inlines);

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