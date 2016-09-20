using System;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public sealed class PrintElementHyperlink : PrintElementSpan
    {
        public Uri Reference { get; set; }
    }
}
