using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementRunHtmlBuilder : IHtmlBuilderBase<PrintElementRun>
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