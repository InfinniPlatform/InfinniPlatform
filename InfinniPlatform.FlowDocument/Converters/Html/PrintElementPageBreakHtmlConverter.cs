using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementPageBreakHtmlConverter : IHtmlBuilderBase<PrintElementPageBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementPageBreak element, StringBuilder result)
        {
            result.Append("<p style=\"page-break-before:always;")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .Append("\">")
                .Append("</p>");
        }
    }
}
