using System;
using System.Text;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для описания поставщика данных в виде регистра системы.
	/// </summary>
	public sealed class RegisterDataProviderInfoConfig
	{
		internal RegisterDataProviderInfoConfig(RegisterDataProviderInfo registerDataProviderInfo)
		{
			if (registerDataProviderInfo == null)
			{
				throw new ArgumentNullException("registerDataProviderInfo");
			}

			_registerDataProviderInfo = registerDataProviderInfo;
		}


		private readonly RegisterDataProviderInfo _registerDataProviderInfo;


		/// <summary>
		/// Тело запроса.
		/// </summary>
		public RegisterDataProviderInfoConfig Body(string value)
		{
			_registerDataProviderInfo.Body = value;

			return this;
		}
	}
}