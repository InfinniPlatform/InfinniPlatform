using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementSectionHtmlConverter : IHtmlBuilderBase<PrintElementSection>
    {
        public override void Build(HtmlBuilderContext context, PrintElementSection element, StringBuilder result)
        {
            result.Append("<div style=\"")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Blocks)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</div>");
        }
    }
}
