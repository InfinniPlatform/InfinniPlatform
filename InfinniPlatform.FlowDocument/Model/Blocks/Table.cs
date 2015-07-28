using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class Table : Block
    {
        public double CellSpacing { get; set; }
        public List<TableRow> Rows;
        public List<TableColumn> Columns;

        public Table()
        {
            Rows = new List<TableRow>();
            Columns = new List<TableColumn>();
        }
    }
}
