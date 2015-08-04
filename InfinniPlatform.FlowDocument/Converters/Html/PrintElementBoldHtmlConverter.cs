using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementBoldHtmlConverter : IHtmlBuilderBase<PrintElementBold>
    {
        public override void Build(HtmlBuilderContext context, PrintElementBold element, StringBuilder result)
        {
            result.Append("<b style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</b>");
        }
    }
}
