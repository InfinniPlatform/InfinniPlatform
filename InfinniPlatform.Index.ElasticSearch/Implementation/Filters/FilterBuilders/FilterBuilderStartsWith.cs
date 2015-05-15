using System;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters.FilterBuilders
{
	/// <summary>
    ///   Builder for "StartsWith" filter applicable for strings
	/// </summary>
	public class FilterBuilderStartsWith : IFilterBuilder
	{
		public BaseFilter Construct<T>(string metadataId, string value) where T:class
		{
            return Filter<T>.Query(q => q.Wildcard(metadataId, value + "*"));
		}
	}
}
