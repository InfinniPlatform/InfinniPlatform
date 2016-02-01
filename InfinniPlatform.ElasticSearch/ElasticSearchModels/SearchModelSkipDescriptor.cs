using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticSearchModels
{
    class SearchModelSkipDescriptor<T> where T:class
	{
		private readonly SearchDescriptor<T> _searchDescriptor;

        public SearchModelSkipDescriptor(SearchDescriptor<T> searchDescriptor)
		{
			_searchDescriptor = searchDescriptor;
		}

		public void ApplyModel(int skip)
		{
		    if (skip > 0)
		    {
		        _searchDescriptor.Skip(skip);
		    }
		}
	}
}
