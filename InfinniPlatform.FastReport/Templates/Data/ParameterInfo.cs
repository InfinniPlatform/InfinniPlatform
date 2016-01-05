using InfinniPlatform.Core.Schema;

namespace InfinniPlatform.FastReport.Templates.Data
{
	/// <summary>
	/// Информация о зарегистрированном параметре отчета.
	/// </summary>
	public sealed class ParameterInfo
	{
		/// <summary>
		/// Тип значения параметра. 
		/// </summary>
		public SchemaDataType Type { get; set; }

		/// <summary>
		/// Наименование параметра.
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// Отображаемый заголовок параметра.
		/// </summary>
		public string Caption { get; set; }

		/// <summary>
		/// Разрешить ввод пустого значения.
		/// </summary>
		public bool AllowNullValue { get; set; }

		/// <summary>
		/// Разрешить указание нескольких значений.
		/// </summary>
		public bool AllowMultiplyValues { get; set; }

		/// <summary>
		/// Доступные значения для выбора.
		/// </summary>
		public IParameterValueProviderInfo AvailableValues { get; set; }

		/// <summary>
		/// Выбранные значения по умолчанию.
		/// </summary>
		public IParameterValueProviderInfo DefaultValues { get; set; }
	}
}