using System.IO;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementListHtmlConverter : IHtmlBuilderBase<PrintElementList>
    {
        public override void Build(HtmlBuilderContext context, PrintElementList element, TextWriter result)
        {
            result.Write("<ul style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);
            result.ApplyListStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Items)
            {
                result.Write("<li style=\"");

                result.Write("padding-left:");
                result.Write(element.MarkerOffsetSize);
                result.Write("px;");

                result.Write("\">");

                context.Build(item, result);

                result.Write("</li>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</ul>");
        }
    }
}
