using System.Text;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementTableHtmlConverter : IHtmlBuilderBase<PrintElementTable>
    {
        public override void Build(HtmlBuilderContext context, PrintElementTable element, StringBuilder result)
        {
            result.Append("<table style=\"")
                .ApplyBaseStyles(element)
                .ApplyBlockStyles(element)
                .Append("border-collapse: collapse;")
                .Append("\">");

            result.ApplySubOrSup(element);

            foreach (var column in element.Columns)
            {
                result.Append("<col width=\"");

                if (column.Size != null)
                {
                    result.Append(column.Size);
                }

                result.Append("\">");
            }

            foreach (var row in element.Rows)
            {
                result.Append("<tr style=\"")
                    .ApplyRowStyles(row)
                    .Append("border-collapse: collapse;")
                    .Append("\">");

                foreach (var cell in row.Cells)
                {
                    result.Append("<td ")
                        .ApplyCellProperties(cell)
                        .Append("style=\"")
                        .ApplyCellStyles(cell)
                        .Append("border-collapse: collapse;")
                        .Append("\">");

                    context.Build(cell.Block, result);

                    result.Append("</td>");
                }

                result.Append("</tr>");
            }

            result.ApplySubOrSupSlash(element);

            result.Append("</table>");
        }
    }
}
