using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public sealed class PrintElementTable : PrintElementBlock
    {
        public PrintElementTable()
        {
            Columns = new List<PrintElementTableColumn>();
            Rows = new List<PrintElementTableRow>();
        }

        public List<PrintElementTableColumn> Columns { get; private set; }
        public List<PrintElementTableRow> Rows { get; private set; }
    }
}
