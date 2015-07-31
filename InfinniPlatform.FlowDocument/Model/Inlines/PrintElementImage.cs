using System.Drawing.Imaging;
using System.IO;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public sealed class PrintElementImage : PrintElementInline
    {
        public PrintElementImage(Stream source)
        {
            Source = source;
        }

        public Stream Source { get; private set; }
        public PrintElementSize Size { get; set; }
        public PrintElementStretch? Stretch { get; set; }

        public void WriteToDisk()
        {
            var i = System.Drawing.Image.FromStream(Source);
            var image = new System.Drawing.Bitmap(i);
            image.Save("C:\\image.bmp", ImageFormat.Bmp);
        }
    }
}