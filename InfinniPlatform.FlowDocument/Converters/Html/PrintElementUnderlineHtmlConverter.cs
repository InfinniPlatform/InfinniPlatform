using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementUnderlineHtmlConverter : IHtmlBuilderBase<PrintElementUnderline>
    {
        public override void Build(HtmlBuilderContext context, PrintElementUnderline element, StringBuilder result)
        {
            result.Append("<ins style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</ins>");
        }
    }
}
