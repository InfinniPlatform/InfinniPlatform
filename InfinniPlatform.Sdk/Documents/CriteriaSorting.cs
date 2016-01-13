namespace InfinniPlatform.Sdk.Documents
{
    public sealed class CriteriaSorting
    {
        public CriteriaSorting(string propertyName, string sortingOrder)
        {
            PropertyName = propertyName;
            SortingOrder = sortingOrder;
        }

        public string PropertyName { get; set; }

        public string SortingOrder { get; set; }

        public override string ToString()
        {
            return $"{PropertyName} {SortingOrder}";
        }
    }
}