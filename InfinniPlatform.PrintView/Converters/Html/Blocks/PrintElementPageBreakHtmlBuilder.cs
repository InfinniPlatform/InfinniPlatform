using System.IO;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html.Blocks
{
    internal sealed class PrintElementPageBreakHtmlBuilder : IHtmlBuilderBase<PrintElementPageBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementPageBreak element, TextWriter result)
        {
            result.Write("<p style=\"page-break-before:always;\"></p>");
        }
    }
}
