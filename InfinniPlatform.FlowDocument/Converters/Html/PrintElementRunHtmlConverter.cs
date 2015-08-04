using System.Text;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class PrintElementRunHtmlConverter : IHtmlBuilderBase<PrintElementRun>
    {
        public override void Build(HtmlBuilderContext context, PrintElementRun element, StringBuilder result)
        {
            result.Append("<span style=\"")
                .ApplyBaseStyles(element)
                .ApplyInlineStyles(element)
                .Append("\">")
                .ApplySubOrSup(element)
                .Append(element.Text)
                .ApplySubOrSupSlash(element)
                .Append("</span>");
        }
    }
}
