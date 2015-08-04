using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementSpanHtmlConverter : IHtmlBuilderBase<PrintElementSpan>
    {
        public override void Build(HtmlBuilderContext context, PrintElementSpan element, StringBuilder result)
        {
            result.Append("<span style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</span>");
        }
    }
}
