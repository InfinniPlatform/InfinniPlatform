using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using FastReport.Table;
using FastReport.Utils;

using InfinniPlatform.FastReport.Templates.Borders;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.FastReport.Templates.Elements;
using InfinniPlatform.FastReport.Templates.Formats;

using TableCell = InfinniPlatform.FastReport.Templates.Elements.TableCell;
using TableColumn = InfinniPlatform.FastReport.Templates.Elements.TableColumn;
using TableRow = InfinniPlatform.FastReport.Templates.Elements.TableRow;

namespace InfinniPlatform.FastReport.ReportTemplateBuilders.Elements
{
	sealed class TableElementTemplateBuilder : IReportObjectTemplateBuilder<TableElement>
	{
		public TableElement BuildTemplate(IReportObjectTemplateBuilderContext context, object reportObject)
		{
			var tableObject = (TableObject)reportObject;

			var tableElement = new TableElement
								   {
									   Border = context.BuildTemplate<Border>(tableObject.Border),
									   Layout = context.BuildTemplate<ElementLayout>(tableObject),

									   Cells = new Collection<TableCell>(),
									   Columns = new List<TableColumn>(),
									   Rows = new List<TableRow>()
								   };

			// Генерация колонок

			var tableColumns = tableObject.Columns;

			if (tableColumns != null)
			{
				for (var columnIndex = 0; columnIndex < tableColumns.Count; ++columnIndex)
				{
					var tableColumn = tableColumns[columnIndex];

					tableElement.Columns.Add(new TableColumn
												 {
													 Index = columnIndex,
													 Width = tableColumn.Width / Units.Millimeters,
													 AutoWidth = tableColumn.AutoSize
												 });
				}
			}

			// Генерация строк

			var tableRows = tableObject.Rows;

			if (tableRows != null)
			{
				for (var rowIndex = 0; rowIndex < tableRows.Count; ++rowIndex)
				{
					var tableRow = tableRows[rowIndex];

					tableElement.Rows.Add(new TableRow
											  {
												  Index = rowIndex,
												  Height = tableRow.Height / Units.Millimeters,
												  AutoHeight = tableRow.AutoSize
											  });

					// Генерация ячеек

					if (tableColumns != null)
					{
						for (var columnIndex = 0; columnIndex < tableColumns.Count; ++columnIndex)
						{
							var tableCell = tableObject[columnIndex, rowIndex];

							var cell = new TableCell
										   {
											   RowIndex = rowIndex,
											   ColumnIndex = columnIndex,
										   };

							if (tableCell != null)
							{
								cell.ColSpan = Math.Max(tableCell.ColSpan, 1);
								cell.RowSpan = Math.Max(tableCell.RowSpan, 1);

								cell.Border = context.BuildTemplate<Border>(tableCell.Border);
								cell.Style = context.BuildTemplate<TextElementStyle>(tableCell);
								cell.Format = context.BuildTemplate<IFormat>(tableCell.Format);
								cell.DataBind = context.BuildTemplate<IDataBind>(tableCell.Text);
							}

							tableElement.Cells.Add(cell);
						}
					}
				}
			}

			return tableElement;
		}
	}
}