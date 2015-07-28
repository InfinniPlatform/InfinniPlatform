using System.Collections.Generic;
using InfinniPlatform.FlowDocument.Model.Inlines;

namespace InfinniPlatform.FlowDocument.Model.Blocks
{
    public class Paragraph : Block
    {
        public double TextIndent { get; set; }
        public List<Inline> Inlines;

        public Paragraph()
        {
            Inlines = new List<Inline>();
        }
    }
}
