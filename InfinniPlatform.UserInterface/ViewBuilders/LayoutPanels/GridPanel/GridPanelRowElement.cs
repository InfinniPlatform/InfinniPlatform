using System;
using System.Collections.Generic;
using System.Windows.Controls;
using InfinniPlatform.UserInterface.Properties;

namespace InfinniPlatform.UserInterface.ViewBuilders.LayoutPanels.GridPanel
{
    /// <summary>
    ///     Строка сетки.
    /// </summary>
    public sealed class GridPanelRowElement
    {
        // Cells

        private int _columnIndex;

        private readonly List<GridPanelCellElement> _cells
            = new List<GridPanelCellElement>();

        private readonly Grid _grid;
        private readonly int _rowIndex;

        public GridPanelRowElement(Grid grid, int rowIndex)
        {
            _grid = grid;
            _rowIndex = rowIndex;
        }

        /// <summary>
        ///     Добавляет ячейку.
        /// </summary>
        public GridPanelCellElement AddCell(int columnSpan)
        {
            if (columnSpan < 1)
            {
                throw new ArgumentException(Resources.GridPanelColumnSpanCannotBeLessOne);
            }

            var cell = new GridPanelCellElement(_grid, _rowIndex, _columnIndex, columnSpan);

            _columnIndex += columnSpan;
            _cells.Add(cell);

            return cell;
        }

        /// <summary>
        ///     Возвращает список ячеек.
        /// </summary>
        public IEnumerable<GridPanelCellElement> GetCells()
        {
            return _cells.AsReadOnly();
        }
    }
}