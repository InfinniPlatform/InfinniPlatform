using System;

namespace InfinniPlatform.PrintView.Model.Inlines
{
    internal class PrintElementHyperlink : PrintElementSpan
    {
        public Uri Reference { get; set; }
    }
}