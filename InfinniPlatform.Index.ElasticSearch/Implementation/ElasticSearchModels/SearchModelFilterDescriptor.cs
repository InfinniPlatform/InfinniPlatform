using InfinniPlatform.Api.Index.SearchOptions;
using InfinniPlatform.Index.ElasticSearch.Implementation.Filters.Extensions;
using InfinniPlatform.Sdk.Environment.Index;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
	public class SearchModelFilterDescriptor<T> where T:class
	{
        private readonly Nest.SearchDescriptor<T> _searchDescriptor;

        public SearchModelFilterDescriptor(Nest.SearchDescriptor<T> searchDescriptor)
		{
			_searchDescriptor = searchDescriptor;
		}

		public void ApplyModel(IFilter criteria)
		{
            criteria.ApplyTo(_searchDescriptor);
		}
	}
}