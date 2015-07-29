using System.IO;

namespace InfinniPlatform.FlowDocument.Model.Inlines
{
    public class Image : Inline
    {
        public Image(Stream source)
        {
            Source = source;
        }

        public Stream Source { get; private set; }
        public double? Width { get; set; }
        public double? Height { get; set; }
        public Stretch Stretch { get; set; }
    }
}