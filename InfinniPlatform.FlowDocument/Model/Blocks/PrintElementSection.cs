using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class PrintElementSection : PrintElementBlock
    {

        public PrintElementSection()
        {
            Blocks = new List<PrintElementBlock>();
        }
        public List<PrintElementBlock> Blocks { get; set; }

    }
}
