using System.IO;

using InfinniPlatform.PrintView.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintHyperlinkHtmlBuilder : HtmlBuilderBase<PrintHyperlink>
    {
        public override void Build(HtmlBuilderContext context, PrintHyperlink element, TextWriter result)
        {
            result.Write("<a href=\"");
            result.Write(element.Reference);
            result.Write("\" style=\"");
            result.ApplyElementStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            if (element.Inlines != null)
            {
                foreach (var item in element.Inlines)
                {
                    context.Build(item, result);
                }
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</a>");
        }
    }
}