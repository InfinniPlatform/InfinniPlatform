using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class PrintElementLineHtmlConverter : IHtmlBuilderBase<PrintElementLine>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLine element, StringBuilder result)
        {
            result.Append("<hr style=\"")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .Append("</hr>");
        }
    }
}
