using System;
using System.Drawing;
using System.IO;

using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Converters.Html.Inlines
{
    internal sealed class PrintElementImageHtmlBuilder : IHtmlBuilderBase<PrintElementImage>
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
