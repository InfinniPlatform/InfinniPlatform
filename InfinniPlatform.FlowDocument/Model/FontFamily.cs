namespace InfinniPlatform.FlowDocument.Model
{
    public sealed class FontFamily
    {
        public FontFamily(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

    }
}