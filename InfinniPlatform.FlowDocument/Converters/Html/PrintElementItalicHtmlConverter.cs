using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementItalicHtmlConverter : IHtmlBuilderBase<PrintElementItalic>
    {
        public override void Build(HtmlBuilderContext context, PrintElementItalic element, StringBuilder result)
        {
            result.Append("<i style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</i>");
        }
    }
}
