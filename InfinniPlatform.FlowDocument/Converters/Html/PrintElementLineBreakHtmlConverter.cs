using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementLineBreakHtmlConverter : IHtmlBuilderBase<PrintElementLineBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLineBreak element, StringBuilder result)
        {
            result.Append("<br style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");
        }
    }
}
