using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model.Font;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public sealed class PrintElementTableRow
    {
        public PrintElementTableRow()
        {
            Cells = new List<PrintElementTableCell>();
        }

        public string Name { get; set; }
        public string Style { get; set; }
        public PrintElementFont Font { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
        public List<PrintElementTableCell> Cells { get; private set; }
    }
}
