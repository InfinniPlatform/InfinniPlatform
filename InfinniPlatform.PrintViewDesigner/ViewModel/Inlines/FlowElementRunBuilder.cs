using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementRunBuilder : IFlowElementBuilderBase<PrintElementRun>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementRun element)
        {
            var elementContent = new Run {Text = element.Text};

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyInlineStyles(elementContent, element);

            return elementContent;
        }
    }
}
