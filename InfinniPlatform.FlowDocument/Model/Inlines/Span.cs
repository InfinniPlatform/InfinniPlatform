using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public class Span : Inline
    {
        public List<Inline> Inlines;

        public Span()
        {
            Inlines = new List<Inline>();
        }
    }
}
