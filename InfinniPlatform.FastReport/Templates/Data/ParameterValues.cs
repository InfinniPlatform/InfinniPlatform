using System.Collections.Generic;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Значения параметра отчета.
	/// </summary>
	public sealed class ParameterValues
	{
		/// <summary>
		/// Доступные значения для выбора.
		/// </summary>
		public IDictionary<string, object> AvailableValues { get; set; }

		/// <summary>
		/// Выбранные значения по умолчанию.
		/// </summary>
		public IDictionary<string, object> DefaultValues { get; set; }
	}
}