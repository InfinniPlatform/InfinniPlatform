namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Информация о зарегистрированном итоге.
	/// </summary>
	public sealed class TotalInfo
	{
		/// <summary>
		/// Наименование итога.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Наименование блока данных.
		/// </summary>
		public string DataBand { get; set; }

		/// <summary>
		/// Наименование блока с итогами.
		/// </summary>
		public string PrintBand { get; set; }

		/// <summary>
		/// Функция расчета итога.
		/// </summary>
		public TotalFunc TotalFunc { get; set; }

		/// <summary>
		/// Агрегируемое выражение.
		/// </summary>
		public IDataBind Expression { get; set; }
	}
}