using InfinniPlatform.ElasticSearch.Filters.Extensions;
using InfinniPlatform.Sdk.Documents;

namespace InfinniPlatform.ElasticSearch.ElasticSearchModels
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