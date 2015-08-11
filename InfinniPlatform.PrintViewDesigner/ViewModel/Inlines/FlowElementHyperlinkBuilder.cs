using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementHyperlinkBuilder : IFlowElementBuilderBase<PrintElementHyperlink>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementHyperlink element)
        {
            var elementContent = new Hyperlink();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            elementContent.NavigateUri = element.Reference;

            foreach (var inline in element.Inlines)
            {
                var inlineContent = context.Build<Inline>(inline);

                elementContent.Inlines.Add(inlineContent);
            }

            return elementContent;
        }
    }
}
