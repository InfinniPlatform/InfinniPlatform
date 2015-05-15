using System.Collections.Generic;

namespace InfinniPlatform.Sdk
{
    /// <summary>
    /// Позволяет описать набор условий сортировки
    /// </summary>
    public sealed class SortingBuilder
    {
        private readonly IList<string> _sortingList;

        public SortingBuilder()
        {
            _sortingList = new List<string>();
        }

        public SortingBuilder AddSorting(string property, string sortOrder = "ascending")
        {
            _sortingList.Add(string.Format("{0} {1}",property,sortOrder));

            return this;
        }

        public IEnumerable<string> GetSorting()
        {
            return _sortingList;
        }
    }
}
