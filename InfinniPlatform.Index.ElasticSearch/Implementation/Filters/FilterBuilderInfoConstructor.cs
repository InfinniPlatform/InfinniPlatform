using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using InfinniPlatform.SearchOptions;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
    public sealed class FilterBuilderInfoConstructor : IFilterBuilderInfoConstructor
    {
	    private readonly FilterBuilderConfig _filterBuilderConfig;

	    public FilterBuilderInfoConstructor(FilterBuilderConfig filterBuilderConfig)
		{
			_filterBuilderConfig = filterBuilderConfig;
		}

	    public string CreateResultFieldInfo(string fieldName)
	    {
            var resultString = fieldName;
            // Ко всем именам полей кроме TimeStamp необходимо добавить префикс Values.
            if (string.CompareOrdinal(fieldName, ElasticConstants.IndexObjectTimeStampField) != 0)
            {
                resultString = ElasticConstants.IndexObjectPath + fieldName;
            }

	        return resultString;
	    }

        public IEnumerable<FilterBuilderInfo> CreateFilterBuilderInfo(IEnumerable<Criteria> criteria)
        {
	        return criteria.Select(cr => new FilterBuilderInfo
            {
                Value = FilterExtension.ConvertToElasticSearchFilterValue(cr.Value),
                MetadataId = cr.CriteriaType != CriteriaType.Script ? CreateResultFieldInfo(cr.Property) : cr.Property,
                FilterBuilder = _filterBuilderConfig.Build(cr.CriteriaType)
            }).ToList();
        }


    }
}