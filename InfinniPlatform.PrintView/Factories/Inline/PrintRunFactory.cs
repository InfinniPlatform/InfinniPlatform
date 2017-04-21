using InfinniPlatform.PrintView.Inline;

namespace InfinniPlatform.PrintView.Factories.Inline
{
    internal class PrintRunFactory : PrintElementFactoryBase<PrintRun>
    {
        public override object Create(PrintElementFactoryContext context, PrintRun template)
        {
            var element = new PrintRun();

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyInlineProperties(element, template, context.ElementStyle);

            element.Text = FactoryHelper.FormatValue(context, template.Text, template.SourceFormat);

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }
    }
}