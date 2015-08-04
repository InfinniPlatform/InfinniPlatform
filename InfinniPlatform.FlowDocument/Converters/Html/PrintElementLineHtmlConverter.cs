using System.IO;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class PrintElementLineHtmlConverter : IHtmlBuilderBase<PrintElementLine>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLine element, TextWriter result)
        {
            result.Write("<hr style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("</hr>");
        }
    }
}
