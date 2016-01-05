using System.Collections.Generic;

using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Core.SearchOptions.Builders
{
    /// <summary>
    ///     Позволяет описать набор условий сортировки
    /// </summary>
    public sealed class SortingBuilder
    {
        private readonly IList<object> _sortingList;

        public SortingBuilder()
        {
            _sortingList = new List<dynamic>();
        }

        public SortingBuilder AddSorting(string property, SortOrder sortOrder = SortOrder.Ascending)
        {
            _sortingList.Add(new
            {
                PropertyName = property,
                SortOrder = sortOrder
            });

            return this;
        }

        public IEnumerable<object> GetSorting()
        {
            return _sortingList;
        }
    }
}