using System.IO;

using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Converters.Html
{
    class PrintElementTableHtmlConverter : IHtmlBuilderBase<PrintElementTable>
    {
        public override void Build(HtmlBuilderContext context, PrintElementTable element, TextWriter result)
        {
            result.Write("<table style=\"");

            result.ApplyBaseStyles(element);
            result.ApplyBlockStyles(element);

            result.Write("border-collapse: collapse;");

            result.Write("\">");

            result.ApplySubOrSup(element);

            foreach (var column in element.Columns)
            {
                result.Write("<col width=\"");

                if (column.Size != null)
                {
                    result.Write(column.Size);
                }

                result.Write("\">");
            }

            foreach (var row in element.Rows)
            {
                result.Write("<tr style=\"");

                result.ApplyRowStyles(row);

                result.Write("border-collapse: collapse;");

                result.Write("\">");

                foreach (var cell in row.Cells)
                {
                    result.Write("<td ");

                    result.ApplyCellProperties(cell);

                    result.Write("style=\"");

                    result.ApplyCellStyles(cell);

                    result.Write("border-collapse: collapse;");

                    result.Write("\">");

                    context.Build(cell.Block, result);

                    result.Write("</td>");
                }

                result.Write("</tr>");
            }

            result.ApplySubOrSupSlash(element);

            result.Write("</table>");
        }
    }
}
