using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementBoldHtmlBuilder : IHtmlBuilderBase<PrintElementBold>
    {
        public override void Build(HtmlBuilderContext context, PrintElementBold element, TextWriter result)
        {
            result.Write("<b style=\"");
            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);
            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</b>");
        }
    }
}