using System.IO;

using InfinniPlatform.FlowDocument.Model.Views;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    public class PrintViewDocumentHtmlBuilder : IHtmlBuilderBase<PrintViewDocument>
    {
        public override void Build(HtmlBuilderContext context, PrintViewDocument element, TextWriter result)
        {
            result.Write("<!DOCTYPE html><html><head></head><body style=\"");

            result.Write("padding-top:");
            result.Write(element.PagePadding.Top);
            result.Write("px;");

            result.Write("padding-right:");
            result.Write(element.PagePadding.Right);
            result.Write("px;");

            result.Write("padding-bottom:");
            result.Write(element.PagePadding.Bottom);
            result.Write("px;");

            result.Write("padding-left:");
            result.Write(element.PagePadding.Left);
            result.Write("px;");

            if (element.PageSize != null && element.PageSize.Width != null)
            {
                result.Write("width:");
                result.Write(element.PageSize.Width);
                result.Write("px;");
            }

            result.ApplyBaseStyles(element);

            result.Write("\">");

            foreach (var item in element.Blocks)
            {
                context.Build(item, result);
            }

            result.Write("</body></html>");
        }
    }
}
