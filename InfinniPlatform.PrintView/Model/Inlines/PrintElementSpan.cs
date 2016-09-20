using System.Collections.Generic;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public class PrintElementSpan : PrintElementInline
    {
        public PrintElementSpan()
        {
            Inlines = new List<PrintElementInline>();
        }

        public List<PrintElementInline> Inlines { get; private set; }
    }
}
