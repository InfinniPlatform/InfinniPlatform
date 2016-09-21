using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementSpanHtmlBuilder : IHtmlBuilderBase<PrintElementSpan>
    {
        public override void Build(HtmlBuilderContext context, PrintElementSpan element, TextWriter result)
        {
            result.Write("<span style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</span>");
        }
    }
}