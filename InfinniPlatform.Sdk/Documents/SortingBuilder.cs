using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents
{
    /// <summary>
    /// Позволяет описать набор условий сортировки
    /// </summary>
    public sealed class SortingBuilder
    {
        public SortingBuilder()
        {
            SortingList = new List<CriteriaSorting>();
        }

        public IList<CriteriaSorting> SortingList { get; }

        public SortingBuilder AddSorting(string property, string sortingOrder = "ascending")
        {
            SortingList.Add(new CriteriaSorting(property, sortingOrder));

            return this;
        }
    }
}