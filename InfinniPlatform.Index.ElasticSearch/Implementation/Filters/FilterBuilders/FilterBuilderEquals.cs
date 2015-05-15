using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "Equals" filter applicable for all datatypes
	/// </summary>
	public class FilterBuilderEquals : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

			return Filter<T>.Term(metadataId, value);
		}

	}
}
