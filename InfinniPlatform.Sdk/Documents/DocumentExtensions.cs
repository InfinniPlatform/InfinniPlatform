using System;
using System.Collections.Generic;

namespace InfinniPlatform.Sdk.Documents
{
    public static class DocumentExtensions
    {
        public static IEnumerable<FilterCriteria> ToFilterCriterias(this Action<FilterBuilder> target)
        {
            if (target != null)
            {
                var filterBuilder = new FilterBuilder();
                target.Invoke(filterBuilder);

                return filterBuilder.CriteriaList;
            }

            return null;
        }

        public static IEnumerable<SortingCriteria> ToSortingCriterias(this Action<SortingBuilder> target)
        {
            if (target != null)
            {
                var sortingBuilder = new SortingBuilder();
                target.Invoke(sortingBuilder);

                return sortingBuilder.SortingList;
            }

            return null;
        }
    }
}