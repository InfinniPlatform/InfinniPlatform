using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model.Blocks;

namespace InfinniPlatform.FlowDocument.Model.Views
{
    public class PrintViewDocument : PrintElement
    {
        public PrintViewDocument()
        {
            Blocks = new List<PrintElementBlock>();
        }
        public double ColumnWidth { get; set; }

        public List<PrintElementBlock> Blocks;

        public double PageWidth { get; set; }
        public double PageHeight { get; set; }
        public Thickness PagePadding { get; set; }

        
    }
}
