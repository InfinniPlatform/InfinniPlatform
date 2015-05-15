namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Правило сортировки свойства источника данных.
	/// </summary>
	public sealed class SortField
	{
		/// <summary>
		/// Путь к свойству источника данных.
		/// </summary>
		public string Property { get; set; }

		/// <summary>
		/// Порядок сортировки свойства источника данных.
		/// </summary>
		public SortOrder SortOrder { get; set; }
	}
}