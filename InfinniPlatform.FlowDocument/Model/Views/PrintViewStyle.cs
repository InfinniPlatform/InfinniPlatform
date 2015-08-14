using InfinniPlatform.FlowDocument.Model.Font;

namespace InfinniPlatform.FlowDocument.Model.Views
{
    public sealed class PrintViewStyle
    {
        public string Name { get; set; }
        public PrintElementFont Font { get; set; }
        public string Foreground { get; set; }
        public string Background { get; set; }
        public PrintElementBorder Border { get; set; }
        public PrintElementThickness Margin { get; set; }
        public PrintElementThickness Padding { get; set; }
        public PrintElementTextAlignment? TextAlignment { get; set; }
        public PrintElementTextDecoration? TextDecoration { get; set; }
    }
}