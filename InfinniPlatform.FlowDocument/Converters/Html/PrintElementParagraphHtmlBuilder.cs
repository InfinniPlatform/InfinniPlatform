using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class PrintElementParagraphHtmlBuilder : IHtmlBuilderBase<PrintElementParagraph>
    {
        public override void Build(HtmlBuilderContext context, PrintElementParagraph element, StringBuilder result)
        {
            result.Append("<p style=\"")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .ApplyParagraphStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</p>");
        }
    }
}
