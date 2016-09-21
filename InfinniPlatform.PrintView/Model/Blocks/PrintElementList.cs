using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Blocks
{
    internal class PrintElementList : PrintElementBlock
    {
        public PrintElementList()
        {
            Items = new List<PrintElementSection>();
        }

        public int? StartIndex { get; set; }

        public PrintElementListMarkerStyle? MarkerStyle { get; set; }

        public double MarkerOffsetSize { get; set; }

        public List<PrintElementSection> Items { get; private set; }
    }
}