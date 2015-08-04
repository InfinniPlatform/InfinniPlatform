using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementHyperlinkHtmlConverter : IHtmlBuilderBase<PrintElementHyperlink>
    {
        public override void Build(HtmlBuilderContext context, PrintElementHyperlink element, StringBuilder result)
        {
            result.Append("<a href=\"")
                .Append(element.Reference)
                .Append("\" style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</a>");
        }
    }
}
