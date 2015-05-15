using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.Reporting.ParameterValueProviders
{
	/// <summary>
	/// Поставщик значений параметров отчета.
	/// </summary>
	interface IParameterValueProvider
	{
		/// <summary>
		/// Получить значения параметра отчета.
		/// </summary>
		/// <param name="providerInfo">Информация о поставщике значений параметров отчета.</param>
		/// <param name="dataSources">Информация о зарегистрированных источниках данных отчета.</param>
		/// <returns>Значения параметров отчета.</returns>
		IDictionary<string, object> GetParameterValues(IParameterValueProviderInfo providerInfo, IEnumerable<DataSourceInfo> dataSources);
	}
}