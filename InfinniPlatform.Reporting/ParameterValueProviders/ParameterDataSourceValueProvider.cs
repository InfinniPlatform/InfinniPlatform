using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataSources;
using InfinniPlatform.Reporting.Properties;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.ParameterValueProviders
{
	/// <summary>
	/// Поставщик значений параметров отчета в виде списка, получаемого из источника данных.
	/// </summary>
	sealed class ParameterDataSourceValueProvider : IParameterValueProvider
	{
		public ParameterDataSourceValueProvider(IDataSource dataSource)
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}

			_dataSource = dataSource;
		}


		private readonly IDataSource _dataSource;


		public IDictionary<string, object> GetParameterValues(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources)
		{
			var dataSourceProvider = (ParameterDataSourceValueProviderInfo)providerInfo;

			var dataSourceName = dataSourceProvider.DataSource;
			var labelProperty = dataSourceProvider.LabelProperty;
			var valueProperty = dataSourceProvider.ValueProperty;

			var dataSource = (dataSources != null) ? dataSources.FirstOrDefault(i => i.Name == dataSourceName) : null;

			if (dataSource == null)
			{
				throw new ArgumentException(string.Format(Resources.DataSourceNotFound, dataSourceName), "providerInfo");
			}

			if (dataSource.Provider == null)
			{
				throw new ArgumentException(string.Format(Resources.DataSourceProviderCannotBeNull, dataSourceName), "providerInfo");
			}

			if (string.IsNullOrWhiteSpace(valueProperty))
			{
				throw new ArgumentException(Resources.ValuePropertyCannotBeNullOrWhiteSpace, "providerInfo");
			}

			var result = new Dictionary<string, object>();

			// Предполагается, что источник данных не зависит от параметров отчета
			var data = _dataSource.GetData(dataSource, null, null);

			if (data != null)
			{
				var index = 0;
				var defaultLabel = string.IsNullOrEmpty(labelProperty);

				foreach (var item in data)
				{
					var label = defaultLabel ? "Item" + index++ : GetPropertyValue(item, labelProperty);

					if (label != null)
					{
						var value = GetPropertyValue(item, valueProperty);

						result[label.ToString()] = value;
					}
				}
			}

			return result;
		}

		private static object GetPropertyValue(object instance, string propertyName)
		{
			var jObject = instance as JObject;

			return (jObject != null) ? jObject[propertyName] : null;
		}
	}
}