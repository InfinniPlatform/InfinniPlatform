using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementLineBreakBuilder : IFlowElementBuilderBase<PrintElementLineBreak>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementLineBreak element)
        {
            var elementContent = new LineBreak();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            return elementContent;
        }
    }
}
