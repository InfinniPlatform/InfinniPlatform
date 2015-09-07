using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public sealed class PrintElementSection : PrintElementBlock
    {
        public PrintElementSection()
        {
            Blocks = new List<PrintElementBlock>();
        }

        public List<PrintElementBlock> Blocks { get; private set; }
    }
}
