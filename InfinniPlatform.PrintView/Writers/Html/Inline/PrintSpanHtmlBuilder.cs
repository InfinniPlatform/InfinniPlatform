using System.IO;

using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintSpanHtmlBuilder : HtmlBuilderBase<PrintSpan>
    {
        public override void Build(HtmlBuilderContext context, PrintSpan element, TextWriter result)
        {
            result.Write("<span style=\"");
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

            result.Write("</span>");
        }
    }
}