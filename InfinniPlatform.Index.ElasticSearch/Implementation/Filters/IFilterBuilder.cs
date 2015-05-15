using Nest;

namespace InfinniPlatform.Index.ElasticSearch.Implementation.Filters
{
	/// <summary>
	///   elastic search base filter provider
	/// </summary>
	public interface IFilterBuilder {

		/// <summary>
		///   returns elastic search base filter
		/// </summary>
		/// <typeparam name="T">Type of indexed entity</typeparam>
		/// <param name="metadataId">indexed type metadata identifier</param>
		/// <param name="value">filter value</param>
		/// <returns>elastic search filter</returns>
		BaseFilter Construct<T>(string metadataId, string value) where T:class ;
	}
}
