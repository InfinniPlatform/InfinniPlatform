using System.Collections.Generic;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Информация о поставщике значений параметров отчета в виде предопределенного списка.
	/// </summary>
	public sealed class ParameterConstantValueProviderInfo : IParameterValueProviderInfo
	{
		/// <summary>
		/// Список значений параметра для выбора.
		/// </summary>
		public IDictionary<string, IDataBind> Items { get; set; }
	}
}