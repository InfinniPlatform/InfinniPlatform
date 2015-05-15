using System;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки информации о зарегистрированном источнике данных.
	/// </summary>
	public sealed class DataProviderInfoConfig
	{
		internal DataProviderInfoConfig(DataSourceInfo dataSourceInfo)
		{
			if (dataSourceInfo == null)
			{
				throw new ArgumentNullException("dataSourceInfo");
			}

			_dataSourceInfo = dataSourceInfo;
		}


		private readonly DataSourceInfo _dataSourceInfo;


		/// <summary>
		/// Поставщик данных в виде реляционной базы данных.
		/// </summary>
		public void Sql(Action<SqlDataProviderInfoConfig> action)
		{
			var dataProvider = new SqlDataProviderInfo();

			var configuration = new SqlDataProviderInfoConfig(dataProvider);
			action(configuration);

			_dataSourceInfo.Provider = dataProvider;
		}

		/// <summary>
		/// Поставщик данных в виде REST-сервиса.
		/// </summary>
		public void Rest(Action<RestDataProviderInfoConfig> action)
		{
			var dataProvider = new RestDataProviderInfo();

			var configuration = new RestDataProviderInfoConfig(dataProvider);
			action(configuration);

			_dataSourceInfo.Provider = dataProvider;
		}

		/// <summary>
		/// Поставщик данных в виде регистра системы.
		/// </summary>
		public void Register(Action<RegisterDataProviderInfoConfig> action)
		{
			var dataProvider = new RegisterDataProviderInfo();

			var configuration = new RegisterDataProviderInfoConfig(dataProvider);
			action(configuration);

			_dataSourceInfo.Provider = dataProvider;
		}
	}
}