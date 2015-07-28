using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Model.Views
{
    public class FlowDocument
    {
        public FontFamily FontFamily { get; set; }
        public double ColumnWidth { get; set; }

        public List<Block> Blocks;

        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
        public Thickness PagePadding { get; set; }

        public FlowDocument()
        {
            Blocks = new List<Block>();
        }
    }
}
