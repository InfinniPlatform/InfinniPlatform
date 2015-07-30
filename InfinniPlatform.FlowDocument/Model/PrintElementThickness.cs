namespace InfinniPlatform.FlowDocument.Model
{
    public sealed class PrintElementThickness
    {
        public PrintElementThickness()
        {
        }

        public PrintElementThickness(double all)
        {
            Left = all;
            Top = all;
            Right = all;
            Bottom = all;
        }

        public PrintElementThickness(double left, double top, double right, double bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public double? Left { get; set; }
        public double? Top { get; set; }
        public double? Right { get; set; }
        public double? Bottom { get; set; }
    }
}