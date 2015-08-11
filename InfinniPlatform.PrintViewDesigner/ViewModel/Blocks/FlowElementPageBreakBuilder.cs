using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementPageBreakBuilder : IFlowElementBuilderBase<PrintElementPageBreak>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementPageBreak element)
        {
            var elementContent = new Paragraph
            {
                FontSize = 0.1,
                BreakPageBefore = true
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            return elementContent;
        }
    }
}
