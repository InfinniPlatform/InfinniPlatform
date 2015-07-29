using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class PrintElementParagraph : PrintElementBlock
    {
        public PrintElementParagraph()
        {
            Inlines = new List<Inline>();
        }
        public double TextIndent { get; set; }
        public List<Inline> Inlines { get; set; }
        public TextDecorations TextDecorations { get; set; }
    }
}
