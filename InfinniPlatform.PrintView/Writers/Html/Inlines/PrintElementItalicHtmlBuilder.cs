using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementItalicHtmlBuilder : IHtmlBuilderBase<PrintElementItalic>
    {
        public override void Build(HtmlBuilderContext context, PrintElementItalic element, TextWriter result)
        {
            result.Write("<i style=\"");
            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</i>");
        }
    }
}