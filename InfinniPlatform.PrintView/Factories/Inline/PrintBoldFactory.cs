using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintBoldFactory : PrintElementFactoryBase<PrintBold>
    {
        public override object Create(PrintElementFactoryContext context, PrintBold template)
        {
            var element = new PrintBold();

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