namespace InfinniPlatform.Api.Tests.Validation.ObjectValidators
{
    public sealed class ValidatorTestCase
    {
        public object ValidationObject { get; set; }

        public object Value { get; set; }

        public override string ToString()
        {
            return string.Format("{{ ValidationObject: {0}, Value: {1} }}", ValidationObject ?? "null", Value ?? "null");
        }
    }
}