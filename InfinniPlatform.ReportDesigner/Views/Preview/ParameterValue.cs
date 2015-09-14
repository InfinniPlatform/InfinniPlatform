namespace InfinniPlatform.ReportDesigner.Views.Preview
{
    internal sealed class ParameterValue
    {
        public ParameterValue(object value, string label)
        {
            Value = value;
            Label = label;
        }

        public object Value { get; private set; }
        public string Label { get; private set; }

        public override string ToString()
        {
            return Label;
        }
    }
}