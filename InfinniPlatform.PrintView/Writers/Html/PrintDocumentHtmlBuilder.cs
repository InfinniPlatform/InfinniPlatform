using System;
using System.IO;

using InfinniPlatform.PrintView.Abstractions;
using InfinniPlatform.PrintView.Factories;

namespace InfinniPlatform.PrintView.Writers.Html
{
    internal class PrintDocumentHtmlBuilder : HtmlBuilderBase<PrintDocument>
    {
        public override void Build(HtmlBuilderContext context, PrintDocument element, TextWriter result)
        {
            result.Write("<!DOCTYPE html><html><head><style>*{margin:0;padding:0;}</style></head><body style=\"");

            var pageSize = element.PageSize;
            var pagePadding = element.PagePadding;

            double width = 0;

            if (pageSize != null)
            {
                width = PrintSizeUnitConverter.ToUnifiedSize(pageSize.Width, pageSize.SizeUnit);
            }

            if (pagePadding != null)
            {
                result.WritePadding(pagePadding);

                width -= (PrintSizeUnitConverter.ToUnifiedSize(pagePadding.Left, pagePadding.SizeUnit)
                          + PrintSizeUnitConverter.ToUnifiedSize(pagePadding.Right, pagePadding.SizeUnit));
            }

            width = Math.Max(width, 0);

            if (width > 0)
            {
                result.WriteSizeProperty("width", width, PrintSizeUnitConverter.UnifiedSizeUnit);
            }

            result.ApplyElementStyles(element);

            result.Write("\">");

            if (element.Blocks != null)
            {
                foreach (var item in element.Blocks)
                {
                    context.Build(item, result);
                }
            }

            result.Write("</body></html>");
        }
    }
}