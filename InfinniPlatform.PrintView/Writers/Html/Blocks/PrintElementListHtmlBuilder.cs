using System.IO;

using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Writers.Html.Blocks
{
    internal class PrintElementListHtmlBuilder : IHtmlBuilderBase<PrintElementList>
    {
        public override void Build(HtmlBuilderContext context, PrintElementList element, TextWriter result)
        {
            result.Write("<div style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("\">");

            if (IsUnorderedList(element))
            {
                result.Write("<ul style=\"");
            }
            else
            {
                result.Write("<ol start=\"");
                if (element.StartIndex != null)
                {
                    result.Write(element.StartIndex);
                }
                result.Write("\" style=\"");
            }

            result.ApplyListStyles(element);

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var item in element.Items)
            {
                result.Write("<li style=\"");

                result.Write("padding-left:");
                result.WriteInvariant(element.MarkerOffsetSize);
                result.Write("px;");

                result.Write("\">");

                context.Build(item, result);

                result.Write("</li>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write(IsUnorderedList(element) ? "</ul>" : "</ol>");

            result.Write("</div>");
        }

        private static bool IsUnorderedList(PrintElementList element)
        {
            return (element.MarkerStyle == null)
                   || (element.MarkerStyle.Value == PrintElementListMarkerStyle.None)
                   || (element.MarkerStyle.Value == PrintElementListMarkerStyle.Disc)
                   || (element.MarkerStyle.Value == PrintElementListMarkerStyle.Circle)
                   || (element.MarkerStyle.Value == PrintElementListMarkerStyle.Square)
                   || (element.MarkerStyle.Value == PrintElementListMarkerStyle.Box);
        }
    }
}