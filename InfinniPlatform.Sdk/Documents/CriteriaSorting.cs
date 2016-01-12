namespace InfinniPlatform.Sdk.Documents
{
    public sealed class CriteriaSorting
    {
        public CriteriaSorting(string property, string sortingOrder)
        {
            Property = property;
            SortingOrder = sortingOrder;
        }

        public string Property { get; set; }

        public string SortingOrder { get; set; }

        public override string ToString()
        {
            return $"{Property} {SortingOrder}";
        }
    }
}