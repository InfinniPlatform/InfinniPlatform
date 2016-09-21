using System.IO;

using InfinniPlatform.PrintView.Model.Views;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal class PrintViewDocumentHtmlBuilder : IHtmlBuilderBase<PrintViewDocument>
    {
        public override void Build(HtmlBuilderContext context, PrintViewDocument element, TextWriter result)
        {
            result.Write("<!DOCTYPE html><html><head><style>*{margin:0;padding:0;}</style></head><body style=\"");

            result.Write("padding-top:");
            result.WriteInvariant(element.PagePadding.Top);
            result.Write("px;");

            result.Write("padding-right:");
            result.WriteInvariant(element.PagePadding.Right);
            result.Write("px;");

            result.Write("padding-bottom:");
            result.WriteInvariant(element.PagePadding.Bottom);
            result.Write("px;");

            result.Write("padding-left:");
            result.WriteInvariant(element.PagePadding.Left);
            result.Write("px;");

            if (element.PageSize != null && element.PageSize.Width != null)
            {
                result.Write("width:");
                result.WriteInvariant(element.PageSize.Width - element.PagePadding.Left - element.PagePadding.Right);
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
