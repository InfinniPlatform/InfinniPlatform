using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementSectionBuilder : IFlowElementBuilderBase<PrintElementSection>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementSection element)
        {
            var elementContent = new Section();

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            foreach (var block in element.Blocks)
            {
                var newBlock = context.Build<Block>(block);
                elementContent.Blocks.Add(newBlock);
            }

            return elementContent;
        }
    }
}
