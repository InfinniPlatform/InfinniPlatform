using System;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public class Hyperlink : Span
    {
        public Uri NavigateUri { get; set; }
    }
}
