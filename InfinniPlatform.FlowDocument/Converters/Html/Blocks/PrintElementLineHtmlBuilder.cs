using System.IO;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html.Blocks
{
    internal sealed class PrintElementLineHtmlBuilder : IHtmlBuilderBase<PrintElementLine>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLine element, TextWriter result)
        {
            result.Write("<hr style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("\">");

            result.Write("</hr>");
        }
    }
}
