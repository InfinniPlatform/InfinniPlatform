using System.IO;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html.Blocks
{
    class PrintElementSectionHtmlBuilder : IHtmlBuilderBase<PrintElementSection>
    {
        public override void Build(HtmlBuilderContext context, PrintElementSection element, TextWriter result)
        {
            result.Write("<div style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Blocks)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</div>");
        }
    }
}
