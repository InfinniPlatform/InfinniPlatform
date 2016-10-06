using System.IO;

using InfinniPlatform.PrintView.Model.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintItalicHtmlBuilder : HtmlBuilderBase<PrintItalic>
    {
        public override void Build(HtmlBuilderContext context, PrintItalic element, TextWriter result)
        {
            result.Write("<i style=\"");
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

            result.Write("</i>");
        }
    }
}