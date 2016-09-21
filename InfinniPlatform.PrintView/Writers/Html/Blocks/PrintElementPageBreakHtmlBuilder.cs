using System.IO;

using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Writers.Html.Blocks
{
    internal class PrintElementPageBreakHtmlBuilder : IHtmlBuilderBase<PrintElementPageBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementPageBreak element, TextWriter result)
        {
            result.Write("<p style=\"page-break-before:always;\"></p>");
        }
    }
}