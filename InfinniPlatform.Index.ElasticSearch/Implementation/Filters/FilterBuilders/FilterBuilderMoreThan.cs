using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "MoreThan" filter applicable for numeric data types and dates
	/// </summary>
	public class FilterBuilderMoreThan : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class {

		    return Filter<T>.Range(r => r.OnField(metadataId).Greater(value));
		}

	}
}
