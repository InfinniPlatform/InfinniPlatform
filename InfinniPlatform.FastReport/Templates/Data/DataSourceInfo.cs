using InfinniPlatform.Api.Schema;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Информация об источнике данных.
	/// </summary>
	public sealed class DataSourceInfo
	{
		/// <summary>
		/// Наименование источника данных.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Описание модели данных.
		/// </summary>
		public DataSchema Schema { get; set; }

		/// <summary>
		/// Поставщик данных.
		/// </summary>
		public IDataProviderInfo Provider { get; set; }
	}
}