using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementParagraphBuilder : IFlowElementBuilderBase<PrintElementParagraph>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementParagraph element)
        {
            var elementContent = new Paragraph();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            if (element.IndentSize != null)
            {
                elementContent.TextIndent = element.IndentSize.Value;
            }

            foreach (var inline in element.Inlines)
            {
                var newInline = context.Build<Inline>(inline);

                if (newInline != null)
                {
                    elementContent.Inlines.Add(newInline);
                }
            }

            return elementContent;
        }
    }
}
