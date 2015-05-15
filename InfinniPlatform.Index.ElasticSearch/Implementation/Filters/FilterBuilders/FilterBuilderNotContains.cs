using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "NotContains" filter applicable for strings
	/// </summary>
	public class FilterBuilderNotContains : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

		    return Filter<T>.Query(q => !q.Wildcard(metadataId, "*" + value + "*"));
		}
	}
}
