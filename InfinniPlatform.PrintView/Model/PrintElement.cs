using InfinniPlatform.FlowDocument.Model.Font;

namespace InfinniPlatform.FlowDocument.Model
{
    public abstract class PrintElement
    {
        public string Name { get; set; }
        public string Style { get; set; }
        public PrintElementFont Font { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
    }
}
