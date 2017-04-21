using System.IO;

using InfinniPlatform.PrintView.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintParagraphHtmlBuilder : HtmlBuilderBase<PrintParagraph>
    {
        public override void Build(HtmlBuilderContext context, PrintParagraph element, TextWriter result)
        {
            result.Write("<p style=\"");
            result.ApplyElementStyles(element);
            result.ApplyBlockStyles(element);
            result.WriteSizeProperty("text-indent", element.IndentSize, element.IndentSizeUnit);
            result.Write("\">");

            result.ApplySubOrSup(element);

            if (element.Inlines != null)
            {
                foreach (var item in element.Inlines)
                {
                    context.Build(item, result);
                }
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</p>");
        }
    }
}