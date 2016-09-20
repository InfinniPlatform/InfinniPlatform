using System.IO;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html.Blocks
{
    internal sealed class PrintElementParagraphHtmlBuilder : IHtmlBuilderBase<PrintElementParagraph>
    {
        public override void Build(HtmlBuilderContext context, PrintElementParagraph element, TextWriter result)
        {
            result.Write("<p style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);
            result.ApplyParagraphStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</p>");
        }
    }
}
