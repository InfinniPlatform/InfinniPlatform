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
            SortingList = new List<SortingCriteria>();
        }

        public IList<SortingCriteria> SortingList { get; }

        public SortingBuilder AddSorting(string property, string sortingOrder = "ascending")
        {
            SortingList.Add(new SortingCriteria(property, sortingOrder));

            return this;
        }
    }
}