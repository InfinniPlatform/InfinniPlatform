using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "MoreThanOrEquals" filter applicable for numeric data types and dates
	/// </summary>
	public class FilterBuilderMoreThanOrEquals : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

			return Filter<T>.Range(r => r.OnField(metadataId).GreaterOrEquals(value));
		}
	}
}
