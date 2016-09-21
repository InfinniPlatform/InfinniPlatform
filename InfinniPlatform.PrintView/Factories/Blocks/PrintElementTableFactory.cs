using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Model;
using InfinniPlatform.PrintView.Model.Blocks;

namespace InfinniPlatform.PrintView.Factories.Blocks
{
    internal class PrintElementTableFactory : IPrintElementFactory
    {
        public object Create(PrintElementBuildContext buildContext, dynamic elementMetadata)
        {
            var element = new PrintElementTable
            {
                Border = new PrintElementBorder
                {
                    Thickness = new PrintElementThickness(1, 1, 0, 0),
                    Color = PrintElementColors.Black
                },
                Margin = BuildHelper.DefaultMargin,
                Padding = BuildHelper.DefaultPadding,
            };

            BuildHelper.ApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyTextProperties(element, elementMetadata);

            BuildHelper.ApplyBlockProperties(element, buildContext.ElementStyle);
            BuildHelper.ApplyBlockProperties(element, elementMetadata);

            // Генерация столбцов таблицы

            CreateTableColumns(buildContext, element, elementMetadata.Columns);

            // Генерация заголовка таблицы

            bool showHeader;

            if (!ConvertHelper.TryToBool(elementMetadata.ShowHeader, out showHeader) || showHeader)
            {
                var tableRow = CreateHeaderTableRow(buildContext, element, elementMetadata.Columns);
                element.Rows.Add(tableRow);
            }

            // Генерация явно объявленных строк таблицы

            CreateStaticTableRows(buildContext, element, elementMetadata.Rows);

            // Генерация строк таблицы по данным источника

            var tableSource = buildContext.ElementSourceValue;

            if (ConvertHelper.ObjectIsCollection(tableSource))
            {
                if (HasCellTemplate(elementMetadata.Columns))
                {
                    foreach (var rowSource in (IEnumerable)tableSource)
                    {
                        var tableRow = CreateDynamicTableRow(buildContext, element, elementMetadata.Columns, rowSource);
                        element.Rows.Add(tableRow);
                    }
                }
            }
            else if (buildContext.IsDesignMode)
            {
                // Отображение шаблона строки в дизайнере

                if (HasCellTemplate(elementMetadata.Columns))
                {
                    var tableRow = CreateDynamicTableRow(buildContext, element, elementMetadata.Columns, null);
                    element.Rows.Add(tableRow);
                }
            }

            BuildHelper.PostApplyTextProperties(element, buildContext.ElementStyle);
            BuildHelper.PostApplyTextProperties(element, elementMetadata);

            return element;
        }

        private static void CreateTableColumns(PrintElementBuildContext buildContext, PrintElementTable table, dynamic columns)
        {
            if (columns != null)
            {
                var autoWidthAvailable = BuildHelper.CalcContentWidth(buildContext.ElementWidth, table.Margin, table.Padding, table.Border.Thickness);
                var autoWidthColumns = 0;

                // Генерация столбцов
                foreach (var column in columns)
                {
                    var tableColumn = new PrintElementTableColumn();

                    double sizeInPixels;

                    // Если указан абсолютный размер
                    if (BuildHelper.TryToSizeInPixels(column.Size, column.SizeUnit, out sizeInPixels))
                    {
                        // Если размер не превышает доступный остаток
                        if (autoWidthAvailable > sizeInPixels)
                        {
                            autoWidthAvailable -= sizeInPixels;
                        }
                        else
                        {
                            sizeInPixels = autoWidthAvailable;

                            autoWidthAvailable = 0;
                        }

                        tableColumn.Size = sizeInPixels;
                    }
                    // Если размер не указан, он высчитывается автоматически
                    else
                    {
                        ++autoWidthColumns;

                        tableColumn.Size = null;
                    }

                    table.Columns.Add(tableColumn);

                    buildContext.MapElement(tableColumn, column);
                }

                if (autoWidthColumns > 0)
                {
                    var autoWidth = Math.Max(autoWidthAvailable, 0) / autoWidthColumns;

                    foreach (var tableColumn in table.Columns)
                    {
                        if (tableColumn.Size == null)
                        {
                            tableColumn.Size = autoWidth;
                        }
                    }
                }
            }
        }

        private static PrintElementTableRow CreateHeaderTableRow(PrintElementBuildContext buildContext, PrintElementTable table, dynamic columns)
        {
            var tableRow = new PrintElementTableRow();

            if (columns != null)
            {
                var columnIndex = 0;

                foreach (var column in columns)
                {
                    var cellMetadata = column.Header;
                    var tableCell = CreateTableCell(buildContext, table, columnIndex, cellMetadata, false);
                    tableRow.Cells.Add(tableCell);

                    ++columnIndex;
                }
            }

            return tableRow;
        }

        private static void CreateStaticTableRows(PrintElementBuildContext buildContext, PrintElementTable table, dynamic rows)
        {
            if (rows != null)
            {
                var rowIndex = 0;
                var columnCount = table.Columns.Count;
                var skippedCells = new List<int>();

                foreach (var rowMetadata in rows)
                {
                    var tableRow = new PrintElementTableRow();

                    if (rowMetadata != null)
                    {
                        var rowStyle = buildContext.FindStyle(rowMetadata.Style);

                        // Установка общих свойств для текста
                        BuildHelper.ApplyTextProperties(tableRow, rowStyle);
                        BuildHelper.ApplyTextProperties(tableRow, rowMetadata);

                        for (var columnIndex = 0; columnIndex < columnCount; ++columnIndex)
                        {
                            var cellOffset = GetCellOffset(columnCount, rowIndex, columnIndex);

                            // Если ячейку не нужно отображать (из-за настроек RowSpan или ColumnSpan)
                            if (!skippedCells.Contains(cellOffset))
                            {
                                var cellMetadata = (rowMetadata.Cells != null) ? Enumerable.ElementAtOrDefault(rowMetadata.Cells, columnIndex) : null;
                                var tableCell = CreateTableCell(buildContext, table, columnIndex, cellMetadata, true);
                                tableRow.Cells.Add(tableCell);

                                AddSkippedCells(skippedCells, columnCount, rowIndex, columnIndex, cellMetadata);
                            }
                        }

                        // Пост-установка общих свойств для текста
                        BuildHelper.PostApplyTextProperties(tableRow, rowStyle);
                        BuildHelper.PostApplyTextProperties(tableRow, rowMetadata);
                    }

                    buildContext.MapElement(tableRow, rowMetadata);

                    table.Rows.Add(tableRow);

                    ++rowIndex;
                }
            }
        }

        private static void AddSkippedCells(ICollection<int> skippedCells, int columnCount, int rowIndex, int columnIndex, dynamic cellMetadata)
        {
            if (cellMetadata != null)
            {
                var rowSpan = GetCellSpan(cellMetadata.RowSpan);
                var columnSpan = GetCellSpan(cellMetadata.ColumnSpan);

                if (rowSpan > 1 || columnSpan > 1)
                {
                    var cellOffset = GetCellOffset(columnCount, rowIndex, columnIndex);

                    for (var r = rowIndex; (r < rowIndex + rowSpan); ++r)
                    {
                        for (var c = columnIndex; (c < columnIndex + columnSpan) && (c < columnCount); ++c)
                        {
                            var skipCellOffset = GetCellOffset(columnCount, r, c);

                            if (skipCellOffset != cellOffset)
                            {
                                skippedCells.Add(skipCellOffset);
                            }
                        }
                    }
                }
            }
        }

        private static int GetCellOffset(int columnCount, int rowIndex, int columnIndex)
        {
            return columnCount * rowIndex + columnIndex;
        }

        private static bool HasCellTemplate(dynamic columns)
        {
            if (columns != null)
            {
                foreach (var column in columns)
                {
                    if (column.CellTemplate != null)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        private static PrintElementTableRow CreateDynamicTableRow(PrintElementBuildContext buildContext, PrintElementTable table, dynamic columns, object rowSource)
        {
            var tableRow = new PrintElementTableRow();

            if (columns != null)
            {
                var rowContext = buildContext.Create(buildContext.ElementWidth);
                rowContext.ElementSourceValue = rowSource;

                var columnIndex = 0;

                foreach (var column in columns)
                {
                    var cellMetadata = column.CellTemplate;
                    var tableCell = CreateTableCell(rowContext, table, columnIndex, cellMetadata, false);
                    tableRow.Cells.Add(tableCell);

                    ++columnIndex;
                }
            }

            return tableRow;
        }

        private static PrintElementTableCell CreateTableCell(PrintElementBuildContext buildContext, PrintElementTable table, int columnIndex, dynamic cellMetadata, bool applySpan)
        {
            var tableCell = new PrintElementTableCell
            {
                ColumnSpan = 1,
                RowSpan = 1,
                Border = new PrintElementBorder
                {
                    Thickness = new PrintElementThickness(0, 0, 1, 1),
                    Color = PrintElementColors.Black
                },
                Padding = BuildHelper.DefaultPadding
            };

            if (cellMetadata != null)
            {
                var cellStyle = buildContext.FindStyle(cellMetadata.Style);

                // Установка общих свойств для текста
                BuildHelper.ApplyTextProperties(tableCell, cellStyle);
                BuildHelper.ApplyTextProperties(tableCell, cellMetadata);

                // Установка общих свойств ячейки таблицы
                BuildHelper.ApplyTableCellProperties(tableCell, cellStyle);
                BuildHelper.ApplyTableCellProperties(tableCell, cellMetadata);

                // Объединение ячеек по горизонтали и вертикали
                if (applySpan)
                {
                    tableCell.ColumnSpan = GetCellSpan(cellMetadata.ColumnSpan);
                    tableCell.RowSpan = GetCellSpan(cellMetadata.RowSpan);
                }

                // Вычисление ширины ячейки для размещения содержимого

                var cellWidth = 0.0;

                for (var i = columnIndex; (i < columnIndex + tableCell.ColumnSpan) && (i < table.Columns.Count); ++i)
                {
                    cellWidth += table.Columns[i].Size.Value;
                }

                cellWidth = BuildHelper.CalcContentWidth(cellWidth, tableCell.Padding, tableCell.Border.Thickness);

                // Создание содержимого и помещение его в ячейку

                var cellContext = buildContext.Create(cellWidth);
                var cellContent = buildContext.ElementBuilder.BuildElement(cellContext, cellMetadata.Block);

                if (cellContent is PrintElementBlock)
                {
                    tableCell.Block = cellContent;
                }

                // Пост-установка общих свойств для текста
                BuildHelper.PostApplyTextProperties(tableCell, cellStyle);
                BuildHelper.PostApplyTextProperties(tableCell, cellMetadata);
            }

            buildContext.MapElement(tableCell, cellMetadata);

            return tableCell;
        }

        private static int GetCellSpan(dynamic cellSpan)
        {
            int cellSpanInt;

            if (ConvertHelper.TryToInt(cellSpan, out cellSpanInt) && cellSpanInt > 0)
            {
                return cellSpanInt;
            }

            return 1;
        }
    }
}