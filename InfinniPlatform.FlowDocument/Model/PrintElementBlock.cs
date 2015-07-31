namespace InfinniPlatform.FlowDocument.Model
{
    public abstract class PrintElementBlock : PrintElement
    {
        public PrintElementBorder Border { get; set; }
        public PrintElementThickness Margin { get; set; }
        public PrintElementThickness Padding { get; set; }
        public PrintElementTextAlignment TextAlignment { get; set; }
    }
}
