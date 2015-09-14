using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml.Linq;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;
using InfinniPlatform.Reporting.DataProviders;
using InfinniPlatform.Reporting.Properties;
using InfinniPlatform.Sdk.Environment.Settings;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace InfinniPlatform.Reporting.DataSources
{
	/// <summary>
	/// Источник данных для REST-сервисов.
	/// </summary>
	sealed class RestDataSource : IDataSource
	{
		private const int DefaultTimeout = 100000;
		private const string DefaultMethod = "GET";
		private const string DefaultContentType = "application/json; encoding='utf-8'";
		private const string DefaultAcceptType = "application/json";


		private static readonly Dictionary<string, Func<Stream, IDataProvider>> KnownDataTypes
			= new Dictionary<string, Func<Stream, IDataProvider>>
				  {
					  { "application/json", CreateJsonDataProvider },
					  { "application/xml", CreateXmlDataProvider }
				  };


		public JArray GetData(DataSourceInfo dataSourceInfo, IEnumerable<ParameterInfo> parameterInfos, IDictionary<string, object> parameterValues)
		{
			var dataProviderInfo = (RestDataProviderInfo)dataSourceInfo.Provider;

			// Адрес сервиса может быть обределен в конфигурации приложения, либо жестко прошит в отчете
			var requestUri = AppSettings.GetValue(dataProviderInfo.RequestUri, dataProviderInfo.RequestUri);

			if (string.IsNullOrWhiteSpace(requestUri))
			{
				throw new ArgumentException(Resources.RequestUriCannotBeNullOrWhiteSpace);
			}

			if (parameterInfos != null)
			{
				// Если адрес сервиса содержит параметры, производится их подстановка. Например, адрес "http://path/to/service/?param1={param1}"
				// будет заменен на адрес "http://path/to/service/?param1=123", если в отчете был определен параметр "param1" и его значение
				// равно "123". Аналогичную подстановку можно произвести и для тела запроса, но в данной реализации это не сделано, так как
				// телом запроса не обязательно является строка (нужно будет анализировать ContentType и кодировку, что достаточно сложно).

				foreach (var parameterInfo in parameterInfos)
				{
					var parameterName = parameterInfo.Name;

					object parameterValue = null;

					if (parameterValues != null)
					{
						parameterValues.TryGetValue(parameterName, out parameterValue);
					}

					requestUri = requestUri.Replace("{" + parameterName + "}", (parameterValue ?? string.Empty).ToString());
				}
			}

			var request = WebRequest.Create(requestUri);

			request.Timeout = (dataProviderInfo.RequestTimeout <= 0)
								  ? DefaultTimeout
								  : dataProviderInfo.RequestTimeout;

			request.Method = string.IsNullOrWhiteSpace(dataProviderInfo.Method)
								 ? DefaultMethod
								 : dataProviderInfo.Method;

			request.ContentType = string.IsNullOrWhiteSpace(dataProviderInfo.ContentType)
									  ? DefaultContentType
									  : dataProviderInfo.ContentType;

			if (!string.IsNullOrEmpty(dataProviderInfo.Body))
			{
				var bodyBytes = dataProviderInfo.Body.ConvertToBytes();

				request.ContentLength = bodyBytes.Length;

				var requestStream = request.GetRequestStream();
				requestStream.Write(bodyBytes, 0, bodyBytes.Length);
				requestStream.Close();
			}

			var response = request.GetResponse();

			var dataStream = response.GetResponseStream();

			return ReadDataStream(dataProviderInfo.AcceptType, dataSourceInfo.Schema, dataStream);
		}


		private static JArray ReadDataStream(string acceptType, DataSchema dataSchema, Stream dataStream)
		{
			var dataProvider = CreateDataProvider(acceptType, dataStream);

			return dataProvider.ToJsonArray(dataSchema);
		}

		private static IDataProvider CreateDataProvider(string acceptType, Stream dataStream)
		{
			if (string.IsNullOrWhiteSpace(acceptType))
			{
				acceptType = DefaultAcceptType;
			}

			Func<Stream, IDataProvider> factory;

			if (KnownDataTypes.TryGetValue(acceptType.Trim().ToLower(), out factory) == false)
			{
				dataStream.Dispose();

				throw new NotSupportedException(string.Format(Resources.AcceptTypeIsNotSupported, acceptType));
			}

			return factory(dataStream);
		}

		private static JsonDataProvider CreateJsonDataProvider(Stream dataStream)
		{
			// Поток должен содержать JSON-массив, представляющий список документов

			JArray jsonArray;

			using (var textReader = new StreamReader(dataStream))
			{
				using (var jsonReader = new JsonTextReader(textReader))
				{
					jsonArray = JArray.Load(jsonReader);
				}
			}

			return new JsonDataProvider(jsonArray);
		}

		private static XmlDataProvider CreateXmlDataProvider(Stream dataStream)
		{
			// Поток должен содержать XML-документ, корневой элемент которого содержит список документов

			var document = XDocument.Load(dataStream);

			return new XmlDataProvider(document.Root);
		}
	}
}