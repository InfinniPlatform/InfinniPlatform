using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.SortBuilders;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
	public class SearchModelSortDescriptor<T> where T:class
	{
		private readonly SearchDescriptor<T> _searchDescriptor;

		public SearchModelSortDescriptor(SearchDescriptor<T> searchDescriptor)
		{
			_searchDescriptor = searchDescriptor;
		}

		public void ApplyModel(IEnumerable<SortOption> criteria)
		{
		    var sortCriteria = criteria as SortOption[] ?? criteria.ToArray();
		    if (criteria != null && sortCriteria.Any())
			{
				new SortContainer().GetSortObject(_searchDescriptor, sortCriteria);
			}
		}
	}
}