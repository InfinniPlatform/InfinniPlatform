using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class TableCell : TextElement
    {
        public TableCell()
        {
            Blocks = new List<Block>();
        }

        public int ColumnSpan { get; set; }

        public int RowSpan { get; set; }

        public Brush BorderBrush { get; set; }

        public Thickness BorderThickness { get; set; }

        public Thickness Padding { get; set; }

        public List<Block> Blocks { get; private set; }
    }
}