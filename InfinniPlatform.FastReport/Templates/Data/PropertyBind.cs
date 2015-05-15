namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Свойство источника данных отчета.
	/// </summary>
	public sealed class PropertyBind : IDataBind
	{
		/// <summary>
		/// Наименование источника данных.
		/// </summary>
		public string DataSource { get; set; }

		/// <summary>
		/// Путь к свойству источника данных.
		/// </summary>
		public string Property { get; set; }
	}
}