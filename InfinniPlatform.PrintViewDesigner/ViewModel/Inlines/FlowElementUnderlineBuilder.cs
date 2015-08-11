using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementUnderlineBuilder : IFlowElementBuilderBase<PrintElementUnderline>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementUnderline element)
        {
            var elementContent = new Underline();

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
