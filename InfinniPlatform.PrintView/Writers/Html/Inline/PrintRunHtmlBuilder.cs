using System.IO;

using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintRunHtmlBuilder : HtmlBuilderBase<PrintRun>
    {
        public override void Build(HtmlBuilderContext context, PrintRun element, TextWriter result)
        {
            result.Write("<span style=\"");
            result.ApplyElementStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            result.Write(element.Text);

            result.ApplySubOrSupSlash(element);

            result.Write("</span>");
        }
    }
}