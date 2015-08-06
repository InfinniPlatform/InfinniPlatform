using System;
using System.Collections.Generic;
using System.IO;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html.Blocks
{
    class PrintElementTableHtmlBuilder : IHtmlBuilderBase<PrintElementTable>
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
                }

                result.Write("\">");
            }

            var skipedCells = new List<PrintElementTableCell>();

            PrintElementTableCell currentCell;

            var rowsCount = element.Rows.Count;

            for (var i = 0; i < rowsCount; i++)
            {
                var cellsCount = element.Rows[i].Cells.Count;

                for (var j = 0; j < cellsCount; j++)
                {
                    currentCell = element.Rows[i].Cells[j];

                    if (currentCell != null && !skipedCells.Contains(currentCell))
                    {
                        if (currentCell.ColumnSpan != null && currentCell.ColumnSpan > 1)
                        {
                            for (var a = j + 1; a < j + currentCell.ColumnSpan; a++)
                            {
                                if (element.Rows[i].Cells.Count > a)
                                {
                                    skipedCells.Add(element.Rows[i].Cells[a]);
                                }
                            }
                        }

                        if (currentCell.RowSpan != null && currentCell.RowSpan > 1)
                        {
                            var colSpan = Math.Max(currentCell.ColumnSpan ?? 1, 1);

                            for (var a = i + 1; a < i + currentCell.RowSpan; a++)
                            {
                                for (var b = j; b < j + colSpan; b++)
                                {
                                    if (element.Rows.Count > a && element.Rows[a].Cells.Count > b)
                                    {
                                        skipedCells.Add(element.Rows[a].Cells[b]);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            foreach (var row in element.Rows)
            {
                result.Write("<tr style=\"");

                result.ApplyRowStyles(row);

                result.Write("border-collapse:collapse;");

                result.Write("\">");

                foreach (var cell in row.Cells)
                {
                    if (!skipedCells.Contains(cell))
                    {
                        result.Write("<td ");

                        result.ApplyCellProperties(cell);

                        result.Write("style=\"");

                        result.ApplyCellStyles(cell);

                        result.Write("border-collapse:collapse;");

                        result.Write("\">");

                        if (cell != null)
                        {
                            context.Build(cell.Block, result);
                        }

                        result.Write("</td>");
                    }
                }

                result.Write("</tr>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</table>");
        }
    }
}
