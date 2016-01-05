using InfinniPlatform.Sdk.Environment.Index;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
    /// <summary>
    /// Построитель запросов на основе поисковой модели
    /// </summary>
	public class ElasticSearchQueryBuilder
    {
        private readonly SearchDescriptor<dynamic> _searchDescriptor;

        public ElasticSearchQueryBuilder(SearchDescriptor<dynamic> searchDescriptor)
        {
            _searchDescriptor = searchDescriptor;
        }

        public SearchDescriptor<dynamic> BuildSearchDescriptor(SearchModel searchModel) 
		{
            var searchModelFilterDescriptor = new SearchModelFilterDescriptor<dynamic>(_searchDescriptor);
		    var filter = searchModel.Filter;
			searchModelFilterDescriptor.ApplyModel(filter);

            var searchModelSortDescriptor = new SearchModelSortDescriptor<dynamic>(_searchDescriptor);
			searchModelSortDescriptor.ApplyModel(searchModel.SortOptions);

            var searchModelFromDescriptor = new SearchModelFromDescriptor<dynamic>(_searchDescriptor);
			searchModelFromDescriptor.ApplyModel(searchModel.FromPage,searchModel.PageSize);

            var searchModelSizeDescriptor = new SearchModelPageSizeDescriptor<dynamic>(_searchDescriptor);
			searchModelSizeDescriptor.ApplyModel(searchModel.PageSize != 0 ? searchModel.PageSize : 10);

            var searchModelSkipDescriptor = new SearchModelSkipDescriptor<dynamic>(_searchDescriptor);
            searchModelSkipDescriptor.ApplyModel(searchModel.Skip);

			return _searchDescriptor;
		}
	}




}