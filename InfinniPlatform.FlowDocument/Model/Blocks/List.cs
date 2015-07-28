using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class List : Block
    {
        public TextMarkerStyle MarkerStyle { get; set; }
        public int StartIndex { get; set; }
        public double MarkerOffset { get; set; }
        public List<ListItem> ListItems;

        public List()
        {
            ListItems = new List<ListItem>();
        }
    }
}
