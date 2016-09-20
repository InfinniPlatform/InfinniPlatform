using System.Collections.Generic;

namespace InfinniPlatform.PrintView.Model.Inlines
{
    internal class PrintElementSpan : PrintElementInline
    {
        public PrintElementSpan()
        {
            Inlines = new List<PrintElementInline>();
        }

        public List<PrintElementInline> Inlines { get; private set; }
    }
}