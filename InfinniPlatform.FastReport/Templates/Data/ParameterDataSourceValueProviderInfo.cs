namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Информация о поставщике значений параметров отчета в виде списка, получаемого из источника данных.
	/// </summary>
	public sealed class ParameterDataSourceValueProviderInfo : IParameterValueProviderInfo
	{
		/// <summary>
		/// Наименование источника данных.
		/// </summary>
		public string DataSource { get; set; }

		/// <summary>
		/// Путь к свойству источника данных, которое будет являться значением параметра.
		/// </summary>
		public string ValueProperty { get; set; }

		/// <summary>
		/// Путь к свойству источника данных, которое будет являться отображаемым значением параметра.
		/// </summary>
		public string LabelProperty { get; set; }
	}
}