using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.Properties;

namespace InfinniPlatform.Reporting.ParameterValueProviders
{
	/// <summary>
	/// Предоставляет интерфейс для получения значений параметров отчета.
	/// </summary>
	sealed class GenericParameterValueProvider : IParameterValueProvider
	{
		private delegate IDictionary<string, object> GetParameterValuesDelegate(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources);

		private readonly Dictionary<Type, GetParameterValuesDelegate> _parameterValueProviders = new Dictionary<Type, GetParameterValuesDelegate>();


		public void RegisterProvider<T>(IParameterValueProvider provider) where T : IParameterValueProviderInfo
		{
			if (provider == null)
			{
				throw new ArgumentNullException("provider");
			}

			var providerType = typeof(T);

			_parameterValueProviders[providerType] = provider.GetParameterValues;
		}

		public IDictionary<string, object> GetParameterValues(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources)
		{
			if (providerInfo == null)
			{
				throw new ArgumentNullException("providerInfo");
			}

			var providerType = providerInfo.GetType();

			GetParameterValuesDelegate getParameterValues;

			if (_parameterValueProviders.TryGetValue(providerType, out getParameterValues) == false)
			{
				throw new NotSupportedException(string.Format(Resources.DataProviderIsNotSupported, providerType.FullName));
			}

			return getParameterValues(providerInfo, dataSources);
		}
	}
}