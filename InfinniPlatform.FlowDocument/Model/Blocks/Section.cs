using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class Section : Block
    {
        public List<Block> Blocks;

        public Section()
        {
            Blocks = new List<Block>();
        }
    }
}
