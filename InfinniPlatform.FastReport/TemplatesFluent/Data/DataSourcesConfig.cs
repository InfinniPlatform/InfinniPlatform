using System;
using System.Collections.Generic;
using System.Linq;

using InfinniPlatform.FastReport.Properties;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс настройки источников данных.
	/// </summary>
	public sealed class DataSourcesConfig
	{
		internal DataSourcesConfig(ICollection<DataSourceInfo> dataSources)
		{
			if (dataSources == null)
			{
				throw new ArgumentNullException("dataSources");
			}

			_dataSources = dataSources;
		}


		private readonly ICollection<DataSourceInfo> _dataSources;


		/// <summary>
		/// Зарегистрировать источник данных.
		/// </summary>
		/// <exception cref="ArgumentNullException"></exception>
		/// <exception cref="ArgumentOutOfRangeException"></exception>
		public DataSourcesConfig Register(string name, Action<DataSourceInfoConfig> action)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			if (_dataSources.Any(i => i.Name == name))
			{
				throw new ArgumentOutOfRangeException(string.Format(Resources.DataSourceAlreadyRegistered, name));
			}

			var dataSourceInfo = new DataSourceInfo { Name = name };

			var configuration = new DataSourceInfoConfig(dataSourceInfo);
			action(configuration);

			_dataSources.Add(dataSourceInfo);

			return this;
		}
	}
}