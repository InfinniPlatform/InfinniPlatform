using System;

namespace InfinniPlatform.Sdk.Documents
{
    [Serializable]
    public sealed class SortingCriteria
    {
        public SortingCriteria(string propertyName, string sortingOrder)
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