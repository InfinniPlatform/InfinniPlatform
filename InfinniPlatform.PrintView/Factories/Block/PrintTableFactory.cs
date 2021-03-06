﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.PrintView.Block;
using InfinniPlatform.PrintView.Defaults;

namespace InfinniPlatform.PrintView.Factories.Block
{
    internal class PrintTableFactory : PrintElementFactoryBase<PrintTable>
    {
        /// <summary>
        /// Пустая ячейка.
        /// </summary>
        private static readonly PrintTableCell EmptyCell = new PrintTableCell();


        public override object Create(PrintElementFactoryContext context, PrintTable template)
        {
            var element = new PrintTable
            {
                ShowHeader = template.ShowHeader ?? PrintViewDefaults.Table.ShowHeader
            };

            FactoryHelper.ApplyElementProperties(element, template, context.ElementStyle);
            FactoryHelper.ApplyBlockProperties(element, template, context.ElementStyle);

            element.Border = template.Border ?? context.ElementStyle?.Border ?? PrintViewDefaults.Table.Border;

            // Создание столбцов таблицы
            CreateColumns(context, element, template.Columns);

            if (template.ShowHeader == true)
            {
                // Создание заголовка таблицы
                CreateHeaderRow(context, element, template.Columns);
            }

            // Генерация явно объявленных строк таблицы
            CreateStaticRows(context, element, template.Rows);

            // Создание строк таблицы по данным источника

            var tableSource = context.ElementSourceValue;

            if (ConvertHelper.ObjectIsCollection(tableSource))
            {
                if (HasCellTemplate(template.Columns))
                {
                    foreach (var rowSource in (IEnumerable)tableSource)
                    {
                        CreateDynamicRow(context, element, template.Columns, rowSource);
                    }
                }
            }
            else if (context.IsDesignMode)
            {
                // Отображение шаблона строки в дизайнере

                if (HasCellTemplate(template.Columns))
                {
                    CreateDynamicRow(context, element, template.Columns, null);
                }
            }

            FactoryHelper.ApplyTextCase(element, element.TextCase);

            return element;
        }


        private static void CreateColumns(PrintElementFactoryContext context, PrintTable table, IEnumerable<PrintTableColumn> columnTemplates)
        {
            if (columnTemplates == null)
            {
                return;
            }

            var autoWidthColumns = 0;
            var autoWidthAvailable = FactoryHelper.CalcContentWidth(context.ElementWidth, table.Margin, table.Padding, table.Border?.Thickness);

            // Создание столбцов
            foreach (var columnTemplate in columnTemplates)
            {
                var columnSize = columnTemplate.Size;
                var columnSizeUnit = columnTemplate.SizeUnit ?? PrintViewDefaults.Text.FontSizeUnit;

                var column = new PrintTableColumn
                {
                    Size = columnSize,
                    SizeUnit = columnSizeUnit
                };

                // Если указан абсолютный размер
                if (columnSize != null)
                {
                    columnSize = PrintSizeUnitConverter.ToUnifiedSize(columnSize.Value, columnSizeUnit);

                    // Если размер не превышает доступный остаток
                    if (autoWidthAvailable > columnSize.Value)
                    {
                        autoWidthAvailable -= columnSize.Value;
                    }
                    else
                    {
                        columnSize = autoWidthAvailable;

                        autoWidthAvailable = 0;
                    }

                    column.Size = columnSize;
                }
                // Если размер не указан, он высчитывается автоматически
                else
                {
                    ++autoWidthColumns;

                    column.Size = null;
                }

                column.SizeUnit = PrintSizeUnitConverter.UnifiedSizeUnit;

                table.Columns.Add(column);

                context.MapElement(column, columnTemplate);
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

        private static void CreateHeaderRow(PrintElementFactoryContext context, PrintTable table, IEnumerable<PrintTableColumn> columnTemplates)
        {
            // Создание строки заголовка на основе настроек по умолчанию

            var headerRow = new PrintTableRow();

            if (columnTemplates != null)
            {
                var columnIndex = 0;

                foreach (var column in columnTemplates)
                {
                    // Создание ячейки заголовка на основе шаблона столбца
                    var cell = CreateCell(context, table, column.Header, columnIndex);

                    headerRow.Cells.Add(cell);

                    ++columnIndex;
                }
            }

            table.Rows.Add(headerRow);
        }

        private static void CreateStaticRows(PrintElementFactoryContext context, PrintTable table, IEnumerable<PrintTableRow> rowTemplates)
        {
            if (rowTemplates == null)
            {
                return;
            }

            var rowIndex = 0;
            var columnCount = table.Columns.Count;
            var ignoredCells = new List<CellCoordinate>();

            foreach (var rowTemplate in rowTemplates)
            {
                var rowStyle = context.FindStyle(rowTemplate.Style);

                // Создание строки на основе шаблона и настроек по умолчанию

                var staticRow = new PrintTableRow
                {
                    Style = rowTemplate.Style,
                    Font = rowTemplate.Font ?? rowStyle?.Font,
                    Foreground = rowTemplate.Foreground ?? rowStyle?.Foreground,
                    Background = rowTemplate.Background ?? rowStyle?.Background,
                    TextCase = rowTemplate.TextCase ?? rowStyle?.TextCase
                };

                for (var columnIndex = 0; columnIndex < columnCount; ++columnIndex)
                {
                    var cellCoordinate = new CellCoordinate(rowIndex, columnIndex);

                    // Если ячейку не нужно отображать (из-за настроек RowSpan или ColumnSpan)
                    if (!ignoredCells.Contains(cellCoordinate))
                    {
                        var cellTemplate = rowTemplate.Cells?[columnIndex];
                        var cell = CreateCell(context, table, cellTemplate, columnIndex);
                        staticRow.Cells.Add(cell);

                        AddIgnoredCells(ignoredCells, cellCoordinate, cellTemplate);
                    }
                }

                // Установка регистра символов текста ячейки
                FactoryHelper.ApplyTextCase(staticRow, staticRow.TextCase);

                context.MapElement(staticRow, rowTemplate);

                table.Rows.Add(staticRow);

                ++rowIndex;
            }
        }

        private static void CreateDynamicRow(PrintElementFactoryContext context, PrintTable table, IEnumerable<PrintTableColumn> columnTemplates, object rowSource)
        {
            // Создание строки на основе настроек по умолчанию

            var dynamicRow = new PrintTableRow();

            if (columnTemplates != null)
            {
                // Определение контекста данных для строки

                var rowContext = context.Create(context.ElementWidth);
                rowContext.ElementSourceValue = rowSource;

                // Создание ячеек строки для каждого столбца

                var columnIndex = 0;

                foreach (var column in columnTemplates)
                {
                    // Создание ячейки на основе шаблона столбца
                    var cell = CreateCell(rowContext, table, column.CellTemplate, columnIndex);
                    dynamicRow.Cells.Add(cell);

                    ++columnIndex;
                }
            }

            // Установка регистра символов текста ячейки
            FactoryHelper.ApplyTextCase(dynamicRow, dynamicRow.TextCase);

            table.Rows.Add(dynamicRow);
        }

        private static PrintTableCell CreateCell(PrintElementFactoryContext context, PrintTable table, PrintTableCell cellTemplate, int columnIndex)
        {
            if (cellTemplate == null)
            {
                cellTemplate = EmptyCell;
            }

            var cellStyle = context.FindStyle(cellTemplate.Style);

            // Создание ячейки на основе шаблона и настроек по умолчанию

            var cell = new PrintTableCell
            {
                Style = cellTemplate.Style,
                Font = cellTemplate.Font ?? cellStyle?.Font,
                Foreground = cellTemplate.Foreground ?? cellStyle?.Foreground,
                Background = cellTemplate.Background ?? cellStyle?.Background,
                TextCase = cellTemplate.TextCase ?? cellStyle?.TextCase,
                Border = cellTemplate.Border ?? cellStyle?.Border ?? PrintViewDefaults.TableCell.Border,
                Padding = cellTemplate.Padding ?? cellStyle?.Padding,
                TextAlignment = cellTemplate.TextAlignment ?? cellStyle?.TextAlignment,
                ColumnSpan = cellTemplate.ColumnSpan ?? 1,
                RowSpan = cellTemplate.RowSpan ?? 1
            };

            // Вычисление ширины ячейки для размещения содержимого

            double cellWidth = 0;

            for (var i = columnIndex; (i < columnIndex + cell.ColumnSpan) && (i < table.Columns.Count); ++i)
            {
                cellWidth += table.Columns[i].Size ?? 0;
            }

            cellWidth = FactoryHelper.CalcContentWidth(cellWidth, cell.Padding, cell.Border?.Thickness);

            // Создание содержимого и помещение его в ячейку

            var cellContext = context.Create(cellWidth);
            var cellContent = context.Factory.BuildElement(cellContext, cellTemplate.Block);

            cell.Block = (PrintBlock)cellContent;

            // Установка регистра символов текста ячейки
            FactoryHelper.ApplyTextCase(cell, cell.TextCase);

            context.MapElement(cell, cellTemplate);

            return cell;
        }

        private static void AddIgnoredCells(ICollection<CellCoordinate> ignoredCells, CellCoordinate currentCellCoordinate, PrintTableCell cellTemplate)
        {
            if (cellTemplate.RowSpan != null)
            {
                for (var i = 1; i < cellTemplate.RowSpan; i++)
                {
                    ignoredCells.Add(new CellCoordinate(currentCellCoordinate.Row + i, currentCellCoordinate.Column));
                }
            }

            if (cellTemplate.ColumnSpan != null)
            {
                for (var i = 1; i < cellTemplate.ColumnSpan; i++)
                {
                    ignoredCells.Add(new CellCoordinate(currentCellCoordinate.Row, currentCellCoordinate.Column + i));
                }
            }
        }

        private static bool HasCellTemplate(IEnumerable<PrintTableColumn> columns)
        {
            return (columns != null) && columns.Any(c => c.CellTemplate != null);
        }
    }
}