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
            _sortingList = new List<CriteriaSorting>();
        }

        private readonly IList<CriteriaSorting> _sortingList;

        public SortingBuilder AddSorting(string property, string sortingOrder = "ascending")
        {
            _sortingList.Add(new CriteriaSorting(property, sortingOrder));

            return this;
        }

        public IEnumerable<CriteriaSorting> GetSorting()
        {
            return _sortingList;
        }
    }
}