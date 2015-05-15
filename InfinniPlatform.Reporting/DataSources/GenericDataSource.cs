using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.Properties;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
	/// <summary>
	/// Настраиваемый источник данных.
	/// </summary>
	sealed class GenericDataSource : IDataSource
	{
		private delegate JArray GetDataDelegate(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues);

		private readonly Dictionary<Type, GetDataDelegate> _dataSources = new Dictionary<Type, GetDataDelegate>();


		public void RegisterDataSource<T>(IDataSource dataSource) where T : IDataProviderInfo
		{
			if (dataSource == null)
			{
				throw new ArgumentNullException("dataSource");
			}

			var dataProviderType = typeof(T);

			_dataSources[dataProviderType] = dataSource.GetData;
		}

		public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
		{
			if (dataSourceInfo == null)
			{
				throw new ArgumentNullException("dataSourceInfo");
			}

			if (dataSourceInfo.Provider == null)
			{
				throw new ArgumentException(Resources.DataProviderInfoCannotBeNull, "dataSourceInfo");
			}

			if (dataSourceInfo.Schema == null)
			{
				throw new ArgumentException(Resources.DataSchemaCannotBeNull, "dataSourceInfo");
			}


			var dataProviderType = dataSourceInfo.Provider.GetType();

			GetDataDelegate getData;

			if (_dataSources.TryGetValue(dataProviderType, out getData) == false)
			{
				throw new NotSupportedException(string.Format(Resources.DataProviderIsNotSupported, dataProviderType.FullName));
			}

			return getData(dataSourceInfo, parameterInfos, parameterValues);
		}
	}
}