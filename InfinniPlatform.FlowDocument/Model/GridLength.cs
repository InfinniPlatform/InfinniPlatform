namespace InfinniPlatform.FlowDocument.Model
{
    public struct GridLength
    {
        private double _unitValue;      //  unit value storage
        private GridUnitType _unitType; //  unit type storage

        public GridLength(double value, GridUnitType type)
        {
            _unitValue = (type == GridUnitType.Auto) ? 0.0 : value;
            _unitType = type;
        }
        public GridUnitType GridUnitType { get { return _unitType; } }
        public double Value { get { return _unitValue; } }
    }
}
