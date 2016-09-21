using System;
using System.IO;

using InfinniPlatform.PrintView.Model.Inlines;

namespace InfinniPlatform.PrintView.Writers.Html.Inlines
{
    internal class PrintElementImageHtmlBuilder : IHtmlBuilderBase<PrintElementImage>
    {
        public override void Build(HtmlBuilderContext context, PrintElementImage element, TextWriter result)
        {
            result.Write("<img src=\"data:image/png;base64,");

            result.Write(Convert.ToBase64String(element.SourceBytes.Value));

            result.Write("\" style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyInlineStyles(element);
            result.ApplyImageStyles(element);

            result.Write("\">");

            result.Write("</img>");
        }
    }
}