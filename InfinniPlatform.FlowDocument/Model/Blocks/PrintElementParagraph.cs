using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public sealed class PrintElementParagraph : PrintElementBlock
    {
        public PrintElementParagraph()
        {
            Inlines = new List<PrintElementInline>();
        }

        public double? IndentSize { get; set; }
        public PrintElementSizeUnit? IndentSizeUnit { get; set; }
        public List<PrintElementInline> Inlines { get; private set; }
    }
}
