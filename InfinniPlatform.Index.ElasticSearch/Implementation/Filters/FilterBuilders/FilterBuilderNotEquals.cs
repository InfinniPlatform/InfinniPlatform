using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "NotEquals" filter applicable for all data types
	/// </summary>
	public class FilterBuilderNotEquals : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

			return Filter<T>.Query(q => !q.Term(metadataId, value));
		}

	}
}
