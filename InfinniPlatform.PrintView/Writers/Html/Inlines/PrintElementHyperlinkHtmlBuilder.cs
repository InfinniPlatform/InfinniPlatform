using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementHyperlinkHtmlBuilder : IHtmlBuilderBase<PrintElementHyperlink>
    {
        public override void Build(HtmlBuilderContext context, PrintElementHyperlink element, TextWriter result)
        {
            result.Write("<a href=\"");
            result.Write(element.Reference);
            result.Write("\" style=\"");
            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</a>");
        }
    }
}