﻿using InfinniPlatform.Api.Index.SearchOptions;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.ElasticSearchModels
{
    /// <summary>
    /// Построитель запросов на получение количества объектов
    /// </summary>
	public class ElasticCountQueryBuilder
    {
        private readonly CountDescriptor<dynamic> _countDescriptor;

        public ElasticCountQueryBuilder(CountDescriptor<dynamic> countDescriptor)
        {
            _countDescriptor = countDescriptor;
        }

        public CountDescriptor<dynamic> BuildCountQueryDescriptor(SearchModel searchModel) 
		{
            var searchModelFilterDescriptor = new CountModelFilterDescriptor<dynamic>(_countDescriptor);
		    var filter = searchModel.Filter;
			searchModelFilterDescriptor.ApplyModel(filter);

			return _countDescriptor;
		}
	}




}