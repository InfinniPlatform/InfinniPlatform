namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Параметр отчета.
	/// </summary>
	public sealed class ParameterBind : IDataBind
	{
		/// <summary>
		/// Наименование параметра.
		/// </summary>
		public string Parameter { get; set; }
	}
}