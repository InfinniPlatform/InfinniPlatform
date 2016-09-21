using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementLineBreakHtmlBuilder : IHtmlBuilderBase<PrintElementLineBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLineBreak element, TextWriter result)
        {
            result.Write("<br>");
        }
    }
}