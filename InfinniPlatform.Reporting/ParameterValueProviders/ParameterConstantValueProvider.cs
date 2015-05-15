using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.Reporting.ParameterValueProviders
{
	/// <summary>
	/// Поставщик значений параметров отчета в виде предопределенного списка.
	/// </summary>
	sealed class ParameterConstantValueProvider : IParameterValueProvider
	{
		public IDictionary<string, object> GetParameterValues(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources)
		{
			var result = new Dictionary<string, object>();

			var constantProvider = (ParameterConstantValueProviderInfo)providerInfo;

			if (constantProvider.Items != null)
			{
				foreach (var item in constantProvider.Items)
				{
					var label = item.Key;
					var value = item.Value as ConstantBind;

					if (value != null)
					{
						result[label] = value.Value;
					}
				}
			}

			return result;
		}
	}
}