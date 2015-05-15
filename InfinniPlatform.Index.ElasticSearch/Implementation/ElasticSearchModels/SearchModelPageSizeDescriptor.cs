using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
	public class SearchModelPageSizeDescriptor<T> where T:class
	{
		private readonly SearchDescriptor<T> _searchDescriptor;

		public SearchModelPageSizeDescriptor(SearchDescriptor<T> searchDescriptor)
		{
			_searchDescriptor = searchDescriptor;
		}

		public void ApplyModel(int pageSize)
		{
			_searchDescriptor.Size(pageSize);
		}
	}
}