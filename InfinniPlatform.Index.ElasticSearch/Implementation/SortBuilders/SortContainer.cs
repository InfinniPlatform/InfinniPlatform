using System.Collections.Generic;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.SortBuilders
{
    public class SortContainer
    {
        public SearchDescriptor<T> GetSortObject<T>(SearchDescriptor<T> searchDescriptor, IEnumerable<SortOption> sortCriteria) where T:class
        {
            foreach (var sortCriterion in sortCriteria)
            {
                var sortOption = sortCriterion.Order;
                var criterion = sortCriterion;

                if (sortOption == Api.SearchOptions.SortOrder.Ascending)
                {
                    searchDescriptor.Sort(s => s.OnField(criterion.Field.AsElasticField() + ".sort").Ascending().IgnoreUnmappedFields().NestedMin());
                }
                else if (sortOption == Api.SearchOptions.SortOrder.Descending)
                {
                    searchDescriptor.Sort(s => s.OnField(criterion.Field.AsElasticField() + ".sort").Descending().IgnoreUnmappedFields().NestedMin());
                }
            }
            return searchDescriptor;
        }
    }
}