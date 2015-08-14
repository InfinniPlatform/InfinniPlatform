using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html.Inlines
{
    internal sealed class PrintElementRunHtmlBuilder : IHtmlBuilderBase<PrintElementRun>
    {
        public override void Build(HtmlBuilderContext context, PrintElementRun element, TextWriter result)
        {
            result.Write("<span style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            result.Write(element.Text);

            result.ApplySubOrSupSlash(element);

            result.Write("</span>");
        }
    }
}
