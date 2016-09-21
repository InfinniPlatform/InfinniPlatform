namespace InfinniPlatform.PrintView.Model
{
    internal struct PrintElementThickness
    {
        public PrintElementThickness(double all)
        {
            _left = all;
            _top = all;
            _right = all;
            _bottom = all;
        }

        public PrintElementThickness(double left, double top, double right, double bottom)
        {
            _left = left;
            _top = top;
            _right = right;
            _bottom = bottom;
        }


        private double _left;
        private double _top;
        private double _right;
        private double _bottom;


        public double Left
        {
            get { return _left; }
            set { _left = value; }
        }

        public double Top
        {
            get { return _top; }
            set { _top = value; }
        }

        public double Right
        {
            get { return _right; }
            set { _right = value; }
        }

        public double Bottom
        {
            get { return _bottom; }
            set { _bottom = value; }
        }
    }
}