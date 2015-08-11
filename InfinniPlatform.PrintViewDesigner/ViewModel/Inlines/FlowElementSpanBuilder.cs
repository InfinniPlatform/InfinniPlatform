using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementSpanBuilder : IFlowElementBuilderBase<PrintElementSpan>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementSpan element)
        {
            var elementContent = new Span();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            foreach (var inline in element.Inlines)
            {
                var inlineContent = context.Build<Inline>(inline);

                elementContent.Inlines.Add(inlineContent);
            }

            return elementContent;
        }
    }
}
