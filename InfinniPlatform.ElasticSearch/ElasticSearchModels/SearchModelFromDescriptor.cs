using Nest;

namespace InfinniPlatform.ElasticSearch.ElasticSearchModels
{
	public class SearchModelFromDescriptor<T> where T:class 
	{
		private readonly SearchDescriptor<T> _searchDescriptor;

		public SearchModelFromDescriptor(SearchDescriptor<T> searchDescriptor)
		{
			_searchDescriptor = searchDescriptor;
		}

		public void ApplyModel(int valueFrom, int pageSize)
		{
			_searchDescriptor.From(valueFrom*pageSize);
		}
	}
}