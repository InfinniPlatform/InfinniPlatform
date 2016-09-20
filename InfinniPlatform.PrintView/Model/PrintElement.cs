using InfinniPlatform.PrintView.Model.Font;

namespace InfinniPlatform.PrintView.Model
{
    internal abstract class PrintElement
    {
        public string Name { get; set; }

        public string Style { get; set; }

        public PrintElementFont Font { get; set; }

        public string Foreground { get; set; }

        public string Background { get; set; }
    }
}