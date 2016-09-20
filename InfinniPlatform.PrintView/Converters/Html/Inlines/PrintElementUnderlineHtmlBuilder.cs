using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html.Inlines
{
    internal sealed class PrintElementUnderlineHtmlBuilder : IHtmlBuilderBase<PrintElementUnderline>
    {
        public override void Build(HtmlBuilderContext context, PrintElementUnderline element, TextWriter result)
        {
            result.Write("<ins style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Inlines)
            {
                context.Build(item, result);
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</ins>");
        }
    }
}
