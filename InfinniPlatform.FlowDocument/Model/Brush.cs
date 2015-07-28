namespace InfinniPlatform.FlowDocument.Model
{
    public class Brush
    {
        public static readonly Brush Black = new Brush("Black");
        public static readonly Brush White = new Brush("White");
        public static readonly Brush Transparent = new Brush("Transparent");

        public Brush(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }
    }
}