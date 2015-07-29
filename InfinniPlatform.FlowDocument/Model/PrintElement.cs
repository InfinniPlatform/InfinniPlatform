namespace InfinniPlatform.FlowDocument.Model
{
    public abstract class PrintElement
    {
        public string Name { get; set; }
        public string Style { get; set; }
        public double FontSize { get; set; }
        public FontFamily FontFamily { get; set; }
        public FontStyle FontStyle { get; set; }
        public FontWeight FontWeight { get; set; }
        public FontStretch FontStretch { get; set; }
    }
}
