using System.IO;

using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintLineBreakHtmlBuilder : HtmlBuilderBase<PrintLineBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintLineBreak element, TextWriter result)
        {
            result.Write("<br>");
        }
    }
}