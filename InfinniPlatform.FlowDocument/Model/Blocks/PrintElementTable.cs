using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class PrintElementTable : PrintElementBlock
    {
        public bool ShowHeader { get; set; }
        public double CellSpacing { get; set; }

        public List<TableRow> Rows { get; set; }

        public List<TableColumn> Columns { get; set; }

        public PrintElementTable()
        {
            Rows = new List<TableRow>();
            Columns = new List<TableColumn>();
            ShowHeader = true;
        }
    }
}
