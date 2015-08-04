using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementBoldHtmlConverter : IHtmlBuilderBase<PrintElementBold>
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
