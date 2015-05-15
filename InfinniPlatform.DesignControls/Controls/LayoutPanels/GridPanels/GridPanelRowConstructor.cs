using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.DesignControls.PropertyDesigner;

namespace InfinniPlatform.DesignControls.Controls.LayoutPanels.GridPanels
{
    internal sealed class GridPanelRowConstructor
    {
        public GridPanelRowConstructor(dynamic[] rows)
        {
            ProcessRows(rows);
        }

        private readonly List<IEnumerable<GridPanelCellMap>> _rows = new List<IEnumerable<GridPanelCellMap>>();

        public List<IEnumerable<GridPanelCellMap>> Rows
        {
            get { return _rows; }
        }


        public IEnumerable<dynamic> GetCells(int rowIndex)
        {
            return _rows[rowIndex];
        } 

        private void ProcessRows(dynamic[] rows)
        {
            var cells = new List<GridPanelCellMap>();
            int rowIndex = 0;
            for (rowIndex = 0; rowIndex < rows.Count(); rowIndex++ )
            {
                dynamic[] rowCells = DesignerExtensions.GetCollection(rows[rowIndex], "Cells").ToArray();
                for (int columnIndex = 0; columnIndex < rowCells.Count(); columnIndex ++)
                {
                    cells.Add(new GridPanelCellMap(rowIndex,columnIndex,rowCells[columnIndex], Convert.ToInt32(rowCells[columnIndex].ColumnSpan)));    
                }
                
            }


            rowIndex = 0;
	        if (rows.Any())
	        {
		        while (true)
		        {
			        int colSpanSum = 0;
			        var rowCells = cells.TakeWhile(s =>
				                                       {
					                                       colSpanSum += Convert.ToInt32(s.ColumnSpan);
					                                       return colSpanSum <= 12;
				                                       }).ToArray();

			        for (int columnIndex = 0; columnIndex < rowCells.Count(); columnIndex++)
			        {
				        var cellMap = rowCells[columnIndex];
				        cellMap.InnerRowIndex = rowIndex;
				        cellMap.InnerColumnIndex = columnIndex;
			        }

			        _rows.Add(rowCells);

			        cells = cells.Except(rowCells).ToList();
			        if (!cells.Any())
			        {
				        break;
			        }
			        rowIndex++;

		        }
	        }
        }

        public int GetRowCount()
        {
            return _rows.Count;
        }

        public int GetColumnCount()
        {
            return _rows.Max(c => c.Count());
        }


        public GridPanelCellMap GetCellMap(int outerRowIndex, int outerColumnIndex)
        {
            GridPanelCellMap outerCell = null;
            foreach (var cellMaps in _rows)
            {
                outerCell =
                    cellMaps.FirstOrDefault(
                        c => c.OuterRowIndex == outerRowIndex && c.OuterColumnIndex == outerColumnIndex);
                if (outerCell != null)
                {
                    break;                    
                }
            }
            return outerCell;
        }
    }
}
