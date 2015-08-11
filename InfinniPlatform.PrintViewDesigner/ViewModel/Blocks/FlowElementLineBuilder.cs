using System.Windows;
using System.Windows.Documents;
using System.Windows.Media;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementLineBuilder : IFlowElementBuilderBase<PrintElementLine>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementLine element)
        {
            var elementContent = new Paragraph
            {
                FontSize = 0.1,
                BorderBrush = Brushes.Black,
                BorderThickness = new Thickness(0, 0, 0, 1)
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            return elementContent;
        }
    }
}
