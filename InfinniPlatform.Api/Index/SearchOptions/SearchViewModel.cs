using System.Collections.Generic;

namespace InfinniPlatform.Api.Index.SearchOptions
{
	/// <summary>
	///   Page search view model
	/// </summary>
	public class SearchViewModel 
	{
		public SearchViewModel(int pageNumber, int pageSize, int hitsCount, IEnumerable<dynamic> items) {
			PageNumber = pageNumber;
			PageSize = pageSize;
			Items = items;
			HitsCount = hitsCount;
		}

		/// <summary>
		///   total count of matches 
		/// </summary>
		public int HitsCount { get; set; }

		/// <summary>
		///   size of data page in items
		/// </summary>
		public int PageSize { get; set; }

		/// <summary>
		///   number of page in total pages by order
		/// </summary>
		public int PageNumber { get; set; }

		/// <summary>
		///  list items belong to page specified
		/// </summary>
		public IEnumerable<dynamic> Items { get; set; }

	}
}