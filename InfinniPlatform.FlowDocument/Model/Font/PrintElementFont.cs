namespace InfinniPlatform.FlowDocument.Model.Font
{
    public sealed class PrintElementFont
    {
        public string Family { get; set; }
        public double? Size { get; set; }
        public PrintElementFontStyle? Style { get; set; }
        public PrintElementFontStretch? Stretch { get; set; }
        public PrintElementFontWeight? Weight { get; set; }
        public PrintElementFontVariant? Variant { get; set; }
    }
}