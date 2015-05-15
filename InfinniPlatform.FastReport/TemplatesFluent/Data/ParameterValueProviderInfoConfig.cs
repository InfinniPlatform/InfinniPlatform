using System;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки информации о поставщике значений параметров отчета.
	/// </summary>
	public sealed class ParameterValueProviderInfoConfig
	{
		internal IParameterValueProviderInfo ValueProviderInfo;

		/// <summary>
		/// Поставщик значений параметров отчета в виде предопределенного списка.
		/// </summary>
		public void ConstantValues(Action<ParameterConstantValueProviderInfoConfig> action)
		{
			var valueProvider = new ParameterConstantValueProviderInfo();

			var configuration = new ParameterConstantValueProviderInfoConfig(valueProvider);
			action(configuration);

			ValueProviderInfo = valueProvider;
		}

		/// <summary>
		/// Поставщик значений параметров отчета в виде списка, получаемого из источника данных.
		/// </summary>
		public void DataSourceValues(string dataSourceName, string valuePropertyPath, string labelPropertyPath = null)
		{
			if (string.IsNullOrWhiteSpace(dataSourceName))
			{
				throw new ArgumentNullException("dataSourceName");
			}

			if (string.IsNullOrWhiteSpace(valuePropertyPath))
			{
				throw new ArgumentNullException("valuePropertyPath");
			}

			ValueProviderInfo = new ParameterDataSourceValueProviderInfo
				                {
					                DataSource = dataSourceName,
					                ValueProperty = valuePropertyPath,
					                LabelProperty = labelPropertyPath ?? valuePropertyPath
				                };
		}
	}
}