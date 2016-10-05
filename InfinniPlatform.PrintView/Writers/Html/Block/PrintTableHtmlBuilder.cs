using System.IO;

using InfinniPlatform.PrintView.Model.Block;

namespace InfinniPlatform.PrintView.Writers.Html.Block
{
    internal class PrintTableHtmlBuilder : HtmlBuilderBase<PrintTable>
    {
        public override void Build(HtmlBuilderContext context, PrintTable element, TextWriter result)
        {
            result.Write("<table style=\"");
            result.ApplyElementStyles(element);
            result.ApplyBlockStyles(element);
            result.Write("border-collapse:collapse;");
            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var column in element.Columns)
            {
                result.Write("<col");
                result.WriteSizeAttribute("width", column.Size, column.SizeUnit);
                result.Write(">");
            }

            foreach (var row in element.Rows)
            {
                result.Write("<tr style=\"");
                ApplyRowStyles(result, row);
                result.Write("border-collapse:collapse;");
                result.Write("\">");

                foreach (var cell in row.Cells)
                {
                    result.Write("<td");
                    result.WriteSizeAttribute("colspan", cell.ColumnSpan, null);
                    result.WriteSizeAttribute("rowspan", cell.RowSpan, null);

                    result.Write(" style=\"");

                    ApplyCellStyles(result, cell);

                    result.Write("border-collapse:collapse;");

                    // По умолчанию содержимое ячейки выравнивается по верхнему краю
                    result.Write("vertical-align:top;");

                    var cellIndex = row.Cells.IndexOf(cell);

                    if (cellIndex >= 0)
                    {
                        var column = element.Columns?[cellIndex];

                        if (column != null)
                        {
                            // Максимальная ширина ячейки равна размеру колонки
                            result.WriteSizeProperty("max-width", column.Size, column.SizeUnit);
                        }
                    }

                    // При переполнении по ширине содержимое скрывается
                    result.Write("overflow:hidden;");

                    result.Write("\">");

                    context.Build(cell.Block, result);

                    result.Write("</td>");
                }

                result.Write("</tr>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</table>");
        }


        private static void ApplyRowStyles(TextWriter writer, PrintTableRow element)
        {
            writer.WriteFont(element.Font);
            writer.WriteForeground(element.Foreground);
            writer.WriteForeground(element.Background);
        }

        private static void ApplyCellStyles(TextWriter writer, PrintTableCell element)
        {
            writer.WriteFont(element.Font);
            writer.WriteForeground(element.Foreground);
            writer.WriteBackground(element.Background);
            writer.WriteBorder(element.Border);
            writer.WritePadding(element.Padding);
            writer.WriteTextAlignment(element.TextAlignment);
        }
    }
}