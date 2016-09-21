using System.IO;

using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Writers.Html.Blocks
{
    internal class PrintElementTableHtmlBuilder : IHtmlBuilderBase<PrintElementTable>
    {
        public override void Build(HtmlBuilderContext context, PrintElementTable element, TextWriter result)
        {
            result.Write("<table style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("border-collapse:collapse;");

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var column in element.Columns)
            {
                result.Write("<col width=\"");

                if (column.Size != null)
                {
                    result.WriteInvariant(column.Size);
                    result.Write("px");
                }

                result.Write("\">");
            }

            foreach (var row in element.Rows)
            {
                result.Write("<tr style=\"");

                result.ApplyRowStyles(row);

                result.Write("border-collapse:collapse;");

                result.Write("\">");

                foreach (var cell in row.Cells)
                {
                    result.Write("<td ");

                    result.ApplyCellProperties(cell);

                    result.Write("style=\"");

                    result.ApplyCellStyles(cell);

                    result.Write("border-collapse:collapse;");

                    //По умолчанию содержимое ячейки выравнивается по верхрнему краю
                    result.Write("vertical-align:top;");

                    //max width and overflow

                    var cellIndex = row.Cells.IndexOf(cell);

                    result.Write("max-width:");

                    if (element.Columns[cellIndex].Size != null)
                    {
                        result.WriteInvariant(element.Columns[cellIndex].Size);
                    }

                    result.Write("px;");
                    result.Write("overflow:hidden;");

                    result.Write("\">");

                    if (cell != null)
                    {
                        context.Build(cell.Block, result);
                    }

                    result.Write("</td>");
                }

                result.Write("</tr>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</table>");
        }
    }
}