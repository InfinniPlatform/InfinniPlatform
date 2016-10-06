using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintItalicFactory : PrintElementFactoryBase<PrintItalic>
    {
        public override object Create(PrintElementFactoryContext context, PrintItalic template)
        {
            var element = new PrintItalic();

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