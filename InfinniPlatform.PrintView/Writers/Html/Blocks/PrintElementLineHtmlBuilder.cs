using System.IO;

using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Writers.Html.Blocks
{
    internal class PrintElementLineHtmlBuilder : IHtmlBuilderBase<PrintElementLine>
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