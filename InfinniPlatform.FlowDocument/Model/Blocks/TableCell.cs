using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class TableCell : PrintElement
    {
        public TableCell()
        {
            Blocks = new List<PrintElementBlock>();
        }

        public int ColumnSpan { get; set; }

        public int RowSpan { get; set; }

        public Brush BorderBrush { get; set; }

        public Thickness BorderThickness { get; set; }

        public Thickness Padding { get; set; }

        public List<PrintElementBlock> Blocks { get; private set; }
        public TextAlignment TextAlignment { get; set; }
    }
}