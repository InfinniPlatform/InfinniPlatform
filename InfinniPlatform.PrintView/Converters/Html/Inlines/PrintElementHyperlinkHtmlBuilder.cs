using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html.Inlines
{
    internal sealed class PrintElementHyperlinkHtmlBuilder : IHtmlBuilderBase<PrintElementHyperlink>
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
