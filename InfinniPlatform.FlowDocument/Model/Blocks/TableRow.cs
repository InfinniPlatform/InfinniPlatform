using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class TableRow : PrintElement
    {
        public List<TableCell> Cells;

        public TableRow()
        {
            Cells = new List<TableCell>();
        }
    }
}
