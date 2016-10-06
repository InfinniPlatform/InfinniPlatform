using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintSectionFactory : PrintElementFactoryBase<PrintSection>
    {
        public override object Create(PrintElementFactoryContext context, PrintSection template)
        {
            var element = new PrintSection();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            // Создание вложенных элементов

            var contentContext = context.CreateContentContext(element.Margin, element.Padding, element.Border?.Thickness);
            var blocks = context.Factory.BuildElements(contentContext, template.Blocks);

            if (blocks != null)
            {
                foreach (PrintBlock block in blocks)
                {
                    element.Blocks.Add(block);
                }
            }

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }
    }
}