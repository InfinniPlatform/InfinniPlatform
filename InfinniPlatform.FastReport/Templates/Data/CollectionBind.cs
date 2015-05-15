using System.Collections.Generic;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Привязка коллекции источника данных.
	/// </summary>
	public sealed class CollectionBind : IDataBind
	{
		/// <summary>
		/// Наименование источника данных.
		/// </summary>
		public string DataSource { get; set; }

		/// <summary>
		/// Путь к колекции источника данных.
		/// </summary>
		public string Property { get; set; }

		/// <summary>
		/// Правила сортировки колекции источника данных.
		/// </summary>
		public ICollection<SortField> SortFields { get; set; }
	}
}