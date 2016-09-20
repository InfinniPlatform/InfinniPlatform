using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Blocks
{
    internal class PrintElementSection : PrintElementBlock
    {
        public PrintElementSection()
        {
            Blocks = new List<PrintElementBlock>();
        }

        public List<PrintElementBlock> Blocks { get; private set; }
    }
}