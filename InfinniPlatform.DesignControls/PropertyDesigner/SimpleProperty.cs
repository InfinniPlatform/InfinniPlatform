namespace InfinniPlatform.DesignControls.PropertyDesigner
{
    public sealed class SimpleProperty : IControlProperty
    {
        private readonly object _defaultValue;

        public SimpleProperty(object defaultValue)
        {
            _defaultValue = defaultValue;
            Value = defaultValue;
        }

        public object DefaultValue
        {
            get { return _defaultValue; }
        }

        public object Value { get; set; }
    }
}