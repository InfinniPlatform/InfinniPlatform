using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Blocks
{
    internal class PrintElementTable : PrintElementBlock
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