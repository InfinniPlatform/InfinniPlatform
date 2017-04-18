using System;
using System.IO;

using InfinniPlatform.PrintView.Abstractions.Inline;

namespace InfinniPlatform.PrintView.Writers.Html.Inline
{
    internal class PrintImageHtmlBuilder : HtmlBuilderBase<PrintImage>
    {
        public override void Build(HtmlBuilderContext context, PrintImage element, TextWriter result)
        {
            result.Write("<img src=\"data:image/png;base64,");

            if (element.Data != null)
            {
                result.Write(Convert.ToBase64String(element.Data));
            }

            result.Write("\" style=\"");

            result.ApplyElementStyles(element);
            result.ApplyInlineStyles(element);

            var imageSize = element.Size;

            if (imageSize != null)
            {
                result.WriteSizeProperty("width", imageSize.Width, imageSize.SizeUnit);
                result.WriteSizeProperty("height", imageSize.Height, imageSize.SizeUnit);
            }

            result.Write("\">");

            result.Write("</img>");
        }
    }
}