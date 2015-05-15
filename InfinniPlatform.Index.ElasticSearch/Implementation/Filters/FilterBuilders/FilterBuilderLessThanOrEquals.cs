using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "LessThanOrEquals" filter applicable for numeric data types and dates
	/// </summary>
	public class FilterBuilderLessThanOrEquals : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

		    return Filter<T>.Range(r => r.OnField(metadataId).LowerOrEquals(value));
		}

	}
}
