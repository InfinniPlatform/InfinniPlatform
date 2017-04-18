using System.IO;

using InfinniPlatform.PrintView.Abstractions.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintPageBreakHtmlBuilder : HtmlBuilderBase<PrintPageBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintPageBreak element, TextWriter result)
        {
            result.Write("<p style=\"page-break-before:always;\"></p>");
        }
    }
}