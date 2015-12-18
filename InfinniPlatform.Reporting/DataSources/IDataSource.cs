using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
	/// <summary>
	/// Источник данных.
	/// </summary>
	interface IDataSource
	{
        /// <summary>
        /// Описание поставщика данных.
        /// </summary>
        Type ProviderType { get; }

        /// <summary>
        /// Получить данные источника.
        /// </summary>
        /// <param name="dataSourceInfo">Информация об источнике данных.</param>
        /// <param name="parameterInfos">Информация о параметрах.</param>
        /// <param name="parameterValues">Значения параметров.</param>
        JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues);
	}
}