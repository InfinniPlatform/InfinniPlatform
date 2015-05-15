using System;
using System.Collections.Generic;
using System.Linq;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
	/// <summary>
	///   Elastic search filters container
	/// </summary>
	public class FilterContainer
	{
		/// <summary>
		///   Construct filter container from builders list
		/// </summary>
		/// <typeparam name="T">Type of indexing entity to filter</typeparam>
		/// <param name="filterBuilders">Builders to create filter elastic search</param>
		/// <returns>elastic search filter expression</returns>
		public Func<FilterDescriptor<T>, BaseFilter> GetFilterObject<T>(IEnumerable<FilterBuilderInfo> filterBuilders ) where T: class
		{
            Func<FilterDescriptor<T>, BaseFilter> result = null;

            Func<FilterDescriptor<T>, IEnumerable<BaseFilter>> getBaseFilters =
                filter => filterBuilders.Select(f => f.ConstructFilter<T>()).ToList();
            if (filterBuilders.Any())
            {
                result = f => f.And(getBaseFilters.Invoke(f).ToArray());
            }
            return result;
        }
        
	}
}