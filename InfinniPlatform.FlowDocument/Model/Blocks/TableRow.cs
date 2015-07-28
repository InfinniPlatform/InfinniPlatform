using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class TableRow : TextElement
    {
        public List<TableCell> Cells;

        public TableRow()
        {
            Cells = new List<TableCell>();
        }
    }
}
