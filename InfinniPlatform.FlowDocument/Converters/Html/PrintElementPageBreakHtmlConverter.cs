using System.IO;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementPageBreakHtmlConverter : IHtmlBuilderBase<PrintElementPageBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementPageBreak element, TextWriter result)
        {
            result.Write("<p style=\"page-break-before:always;");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("\">");

            result.Write("</p>");
        }
    }
}
