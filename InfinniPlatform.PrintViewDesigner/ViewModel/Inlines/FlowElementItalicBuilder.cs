using System.Windows.Documents;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.PrintViewDesigner.ViewModel.Inlines
{
    sealed class FlowElementItalicBuilder : IFlowElementBuilderBase<PrintElementItalic>
    {
        public override object Build(FlowElementBuilderContext context, PrintElementItalic element)
        {
            var elementContent = new Italic();

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
