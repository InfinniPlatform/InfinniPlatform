using System;
using System.Text;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для описания поставщика данных в виде REST-сервиса.
	/// </summary>
	public sealed class RestDataProviderInfoConfig
	{
		internal RestDataProviderInfoConfig(RestDataProviderInfo restDataProviderInfo)
		{
			if (restDataProviderInfo == null)
			{
				throw new ArgumentNullException("restDataProviderInfo");
			}

			_restDataProviderInfo = restDataProviderInfo;
		}


		private readonly RestDataProviderInfo _restDataProviderInfo;


		/// <summary>
		/// Время ожидания (в миллисекундах) перед завершением попытки выполнить запрос и созданием ошибки.
		/// </summary>
		public RestDataProviderInfoConfig RequestTimeout(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_restDataProviderInfo.RequestTimeout = value;

			return this;
		}

		/// <summary>
		/// Наименование в конфигурации шаблона запрашиваемого URI или сам URI.
		/// </summary>
		public RestDataProviderInfoConfig RequestUri(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_restDataProviderInfo.RequestUri = value;

			return this;
		}

		/// <summary>
		/// Метод запроса данных (GET, POST, PUT, DELETE).
		/// </summary>
		public RestDataProviderInfoConfig Method(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_restDataProviderInfo.Method = value;

			return this;
		}

		/// <summary>
		/// Тип данных тела запроса в формате MIME.
		/// </summary>
		public RestDataProviderInfoConfig ContentType(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_restDataProviderInfo.ContentType = value;

			return this;
		}

		/// <summary>
		/// Тело запроса.
		/// </summary>
		public RestDataProviderInfoConfig Body(string value)
		{
			_restDataProviderInfo.Body = value;

			return this;
		}
	}
}