using System.IO;

using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintLineHtmlBuilder : HtmlBuilderBase<PrintLine>
    {
        public override void Build(HtmlBuilderContext context, PrintLine element, TextWriter result)
        {
            result.Write("<hr style=\"");
            result.ApplyElementStyles(element);
            result.ApplyBlockStyles(element);
            result.Write("\">");
            result.Write("</hr>");
        }
    }
}