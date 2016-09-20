using System;
using System.Drawing;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public sealed class PrintElementImage : PrintElementInline
    {
        public PrintElementImage(Bitmap source)
        {
            Source = source;
            SourceBytes = new Lazy<byte[]>(() => (byte[])new ImageConverter().ConvertTo(source, typeof(byte[])));
        }

        public Bitmap Source { get; private set; }
        public Lazy<byte[]> SourceBytes { get; private set; }

        public PrintElementSize Size { get; set; }

        public PrintElementStretch? Stretch { get; set; }
    }
}