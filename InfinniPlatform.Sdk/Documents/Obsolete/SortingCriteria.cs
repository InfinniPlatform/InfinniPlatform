using System;
using System.Diagnostics;

namespace InfinniPlatform.Sdk.Documents.Obsolete
{
    [Serializable]
    [DebuggerDisplay("{PropertyName}, {SortingOrder}")]
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