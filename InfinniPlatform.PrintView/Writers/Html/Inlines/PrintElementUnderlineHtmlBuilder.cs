using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementUnderlineHtmlBuilder : IHtmlBuilderBase<PrintElementUnderline>
    {
        public override void Build(HtmlBuilderContext context, PrintElementUnderline element, TextWriter result)
        {
            result.Write("<ins style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</ins>");
        }
    }
}