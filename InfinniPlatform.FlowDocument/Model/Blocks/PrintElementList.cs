using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class PrintElementList : PrintElementBlock
    {
        public TextMarkerStyle MarkerStyle { get; set; }
        public int StartIndex { get; set; }
        public double MarkerOffset { get; set; }
        public List<ListItem> ListItems { get; set; }

        public PrintElementList()
        {
            ListItems = new List<ListItem>();
        }
    }
}
