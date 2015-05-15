using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Dynamic;
using InfinniPlatform.Api.RestApi.DataApi;
using InfinniPlatform.FastReport.Templates.Data;

using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
	/// <summary>
	/// Источник данных для регистров системы.
	/// </summary>
	sealed class RegisterDataSource : IDataSource
	{
		public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
		{
			var dataProviderInfo = (RegisterDataProviderInfo)dataSourceInfo.Provider;
			var requestBody = dataProviderInfo.Body;

			if (string.IsNullOrEmpty(requestBody) == false && parameterInfos != null)
			{
				// Если тело запроса содержит параметры, производится их подстановка. Например, строка "{param1}" будет
				// заменена на "123", если в отчете был определен параметр "param1" и его значение равно "123".

				foreach (var parameterInfo in parameterInfos)
				{
					var parameterName = parameterInfo.Name;

					object parameterValue = null;

					if (parameterValues != null)
					{
						parameterValues.TryGetValue(parameterName, out parameterValue);
					}

					requestBody = requestBody.Replace("{" + parameterName + "}", (parameterValue ?? string.Empty).ToString());
				}
			}

			IEnumerable<dynamic> result = new DocumentApi().GetDocumentByQuery(requestBody);

		    return JArray.FromObject(result.Select(r => r.Result));
		}
	}
}