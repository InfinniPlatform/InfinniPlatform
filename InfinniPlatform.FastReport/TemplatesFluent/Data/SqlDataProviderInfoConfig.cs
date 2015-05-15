using System;
using System.Text;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для описания поставщика данных в виде реляционной базы данных.
	/// </summary>
	public sealed class SqlDataProviderInfoConfig
	{
		internal SqlDataProviderInfoConfig(SqlDataProviderInfo sqlDataProviderInfo)
		{
			if (sqlDataProviderInfo == null)
			{
				throw new ArgumentNullException("sqlDataProviderInfo");
			}

			_sqlDataProviderInfo = sqlDataProviderInfo;
		}


		private readonly SqlDataProviderInfo _sqlDataProviderInfo;


		/// <summary>
		/// Тип SQL-сервера.
		/// </summary>
		public SqlDataProviderInfoConfig ServerType(SqlServerType value)
		{
			_sqlDataProviderInfo.ServerType = value;

			return this;
		}

		/// <summary>
		/// Время ожидания (в миллисекундах) перед завершением попытки выполнить команду и созданием ошибки.
		/// </summary>
		public SqlDataProviderInfoConfig CommandTimeout(int value)
		{
			if (value <= 0)
			{
				throw new ArgumentOutOfRangeException("value");
			}

			_sqlDataProviderInfo.CommandTimeout = value;

			return this;
		}

		/// <summary>
		/// Наименование в конфигурации строки подключения к базе данных или сама строка подключения.
		/// </summary>
		public SqlDataProviderInfoConfig ConnectionString(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_sqlDataProviderInfo.ConnectionString = value;

			return this;
		}

		/// <summary>
		/// Команда выборки данных.
		/// </summary>
		public SqlDataProviderInfoConfig SelectCommand(string value)
		{
			if (string.IsNullOrWhiteSpace(value))
			{
				throw new ArgumentNullException("value");
			}

			_sqlDataProviderInfo.SelectCommand = value;

			return this;
		}
	}
}