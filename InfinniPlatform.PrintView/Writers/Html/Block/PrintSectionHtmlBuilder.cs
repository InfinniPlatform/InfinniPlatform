using System.IO;

using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintSectionHtmlBuilder : HtmlBuilderBase<PrintSection>
    {
        public override void Build(HtmlBuilderContext context, PrintSection element, TextWriter result)
        {
            result.Write("<div style=\"");
            result.ApplyElementStyles(element);
            result.ApplyBlockStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            if (element.Blocks != null)
            {
                foreach (var item in element.Blocks)
                {
                    context.Build(item, result);
                }
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</div>");
        }
    }
}