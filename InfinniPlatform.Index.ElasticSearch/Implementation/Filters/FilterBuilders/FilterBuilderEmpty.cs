using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "Empty" filter applicable for string values
	/// </summary>
	public class FilterBuilderEmpty : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

            // ?* - means contain something (at least 1 char)
            return Filter<T>.Query(q => !q.Wildcard(metadataId, "?*"));
		}

	}
}
