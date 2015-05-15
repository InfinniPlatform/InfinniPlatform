using System;
using InfinniPlatform.Index.ElasticSearch.Implementation.ElasticProviders;
using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
	public class FilterBuilderInfo 
	{
		/// <summary>
		///   indexed metadata identifier
		/// </summary>
		public string MetadataId { get; set; }

		/// <summary>
		///   value of filter
		/// </summary>
		public string Value { get; set; }

		/// <summary>
		///   filter builder for filter construct
		/// </summary>
		public IFilterBuilder FilterBuilder { get; set; }		

		/// <summary>
		///   Constructs elastic search filter implementation from metadata identifier and value
		/// </summary>
		/// <typeparam name="T">type of indexed entity that should be filtered</typeparam>
		/// <returns>elastic search filter</returns>
		public BaseFilter ConstructFilter<T>() where T:class {

            if (string.CompareOrdinal(MetadataId, ElasticConstants.IndexObjectTimeStampField) != 0)
            {
                if (MetadataId.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).Length < 2)
                {
                    throw new ArgumentException("MetadataId has incorrect structure. MetadataId should follow pattern: xxx.yyy, but was " + MetadataId);
                }
            }
            var result = FilterBuilder.Construct<T>(MetadataId, Value);
			return result;
		}
	}
}