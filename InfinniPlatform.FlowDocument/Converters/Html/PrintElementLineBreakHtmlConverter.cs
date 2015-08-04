using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementLineBreakHtmlConverter : IHtmlBuilderBase<PrintElementLineBreak>
    {
        public override void Build(HtmlBuilderContext context, PrintElementLineBreak element, TextWriter result)
        {
            result.Write("<br style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);

            result.Write("\">");
        }
    }
}
