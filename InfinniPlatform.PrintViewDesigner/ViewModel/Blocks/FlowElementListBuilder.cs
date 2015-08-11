using System.Windows;
using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Blocks
{
    sealed class FlowElementListBuilder : IFlowElementBuilderBase<PrintElementList>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementList element)
        {
            var elementContent = new List
            {
                MarkerStyle = TextMarkerStyle.None,
                MarkerOffset = element.MarkerOffsetSize
            };

            FlowElementBuilderHelper.ApplyBaseStyles(elementContent, element);
            FlowElementBuilderHelper.ApplyBlockStyles(elementContent, element);

            if (element.StartIndex != null)
            {
                elementContent.StartIndex = element.StartIndex.Value;
            }

            if (element.MarkerStyle != null)
            {
                elementContent.MarkerStyle = FlowElementBuilderHelper.GetMarkerStyle(element.MarkerStyle.Value);
            }

            foreach (var item in element.Items)
            {
                var itemContent = context.Build<Block>(item);

                if (itemContent != null)
                {
                    var listItem = new ListItem();
                    listItem.Blocks.Add(itemContent);
                    elementContent.ListItems.Add(listItem);
                }
            }

            return elementContent;
        }
    }
}
