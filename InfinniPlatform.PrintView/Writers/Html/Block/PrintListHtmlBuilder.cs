using System.IO;

using InfinniPlatform.PrintView.Abstractions.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintListHtmlBuilder : HtmlBuilderBase<PrintList>
    {
        public override void Build(HtmlBuilderContext context, PrintList element, TextWriter result)
        {
            result.Write("<div style=\"");
            result.ApplyElementStyles(element);
            result.ApplyBlockStyles(element);
            result.Write("\">");

            var isUnorderedList = IsUnorderedList(element);

            if (isUnorderedList)
            {
                result.Write("<ul style=\"");
            }
            else
            {
                result.Write("<ol");
                result.WriteSizeAttribute("start", element.StartIndex, null);
                result.Write(" style=\"");
            }

            result.WriteEnumProperty("list-style-type", element.MarkerStyle);
            result.Write("\">");

            result.ApplySubOrSup(element);

            if (element.Items != null)
            {
                foreach (var item in element.Items)
                {
                    result.Write("<li style=\"");
                    result.WriteSizeProperty("padding-left", element.MarkerOffsetSize, element.MarkerOffsetSizeUnit);
                    result.Write("\">");

                    context.Build(item, result);

                    result.Write("</li>");
                }
            }

            result.ApplySubOrSupSlash(element);

            result.Write(isUnorderedList ? "</ul>" : "</ol>");

            result.Write("</div>");
        }

        private static bool IsUnorderedList(PrintList element)
        {
            return (element.MarkerStyle == null)
                   || (element.MarkerStyle.Value == PrintListMarkerStyle.None)
                   || (element.MarkerStyle.Value == PrintListMarkerStyle.Disc)
                   || (element.MarkerStyle.Value == PrintListMarkerStyle.Circle)
                   || (element.MarkerStyle.Value == PrintListMarkerStyle.Square)
                   || (element.MarkerStyle.Value == PrintListMarkerStyle.Box);
        }
    }
}