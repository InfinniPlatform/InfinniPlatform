using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels
{
    public sealed class GridPanelCellMap
    {
        private readonly int _outerRowIndex;
        private readonly int _outerColumnIndex;
        private readonly dynamic _cell;
        private readonly int _columnSpan;

        public GridPanelCellMap(int outerRowIndex, int outerColumnIndex, dynamic cell, int columnSpan)
        {
            _outerRowIndex = outerRowIndex;
            _outerColumnIndex = outerColumnIndex;
            _cell = cell;
            _columnSpan = columnSpan;
        }

        public dynamic Cell
        {
            get { return _cell; }
        }

        public int ColumnSpan
        {
            get { return _columnSpan; }
        }

        public int OuterColumnIndex
        {
            get { return _outerColumnIndex; }
        }

        public int OuterRowIndex
        {
            get { return _outerRowIndex; }
        }

        public int InnerRowIndex { get; set; }

        public int InnerColumnIndex { get; set; }
    }
}
