using System;
using System.Linq;

using FastReport;
using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Elements;

using TableCell = FastReport.Table.TableCell;
using TableColumn = FastReport.Table.TableColumn;
using TableRow = FastReport.Table.TableRow;

namespace InfinniPlatform.FastReport.ReportObjectBuilders.Elements
{
	sealed class TableElementBuilder : IReportObjectBuilder<TableElement>
	{
		public void BuildObject(IReportObjectBuilderContext context, TableElement template, object parent)
		{
			var container = (IParent)parent;

			var tableElement = context.CreateObject<TableObject>();
			container.AddChild(tableElement);

			// Генерация колонок

			var tempalteColumns = template.Columns;

			if (tempalteColumns != null)
			{
				foreach (var column in tempalteColumns)
				{
					var tableColumn = context.CreateObject<TableColumn>();
					tableColumn.AutoSize = column.AutoWidth;
					tableColumn.Width = column.Width * Units.Millimeters;

					tableElement.Columns.Add(tableColumn);
				}
			}

			// Генерация строк

			var templateRows = template.Rows;

			if (templateRows != null)
			{
				foreach (var row in templateRows)
				{
					var rowIndex = row.Index;

					var tableRow = context.CreateObject<TableRow>();
					tableRow.AutoSize = row.AutoHeight;
					tableRow.Height = row.Height * Units.Millimeters;

					// Генерация ячеек

					if (tempalteColumns != null)
					{
						foreach (var column in tempalteColumns)
						{
							var columnIndex = column.Index;

							var tableCell = context.CreateObject<TableCell>();

							var cell = template.Cells.FirstOrDefault(i => i.RowIndex == rowIndex && i.ColumnIndex == columnIndex);

							if (cell != null)
							{
								tableCell.ColSpan = Math.Max(cell.ColSpan, 1);
								tableCell.RowSpan = Math.Max(cell.RowSpan, 1);

								context.BuildObject(cell.Border, tableCell);
								context.BuildObject(cell.Style, tableCell);
								context.BuildObject(cell.Format, tableCell);
								context.BuildObject(cell.DataBind, tableCell);
							}

							tableRow.AddChild(tableCell);
						}
					}

					tableElement.Rows.Add(tableRow);
				}
			}

			context.BuildObject(template.Border, tableElement);
			context.BuildObject(template.Layout, tableElement);

			tableElement.Width = tableElement.CalcWidth() * Units.Millimeters;
			tableElement.Height = tableElement.CalcHeight();
		}
	}
}