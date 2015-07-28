using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class ListItem : TextElement
    {
        public List<Block> Blocks;

        public ListItem()
        {
            Blocks = new List<Block>();
        }
    }
}
