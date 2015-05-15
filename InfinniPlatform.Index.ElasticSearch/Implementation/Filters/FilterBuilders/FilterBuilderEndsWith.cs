using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "EndsWith" filter applicable for string values
	/// </summary>
	public class FilterBuilderEndsWith : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

			return Filter<T>.Query(q => q.Wildcard(metadataId, "*" + value.ToLowerInvariant()));
		}
	}
}
