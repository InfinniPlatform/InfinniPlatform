using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementListHtmlConverter : IHtmlBuilderBase<PrintElementList>
    {
        public override void Build(HtmlBuilderContext context, PrintElementList element, StringBuilder result)
        {
            result.Append("<ul style=\"")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .ApplyListStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Items)
            {
                result.Append("<li style=\"");

                result.Append("padding-left:");
                result.Append(element.MarkerOffsetSize);
                result.Append("px;");

                result.Append("\">");

                context.Build(item, result);

                result.Append("</li>");
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</ul>");
        }
    }
}
