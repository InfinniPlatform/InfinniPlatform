using System;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	public sealed class DataBindConfig
	{
		internal DataBindConfig(Action<IDataBind> dataBindSetter)
		{
			if (dataBindSetter == null)
			{
				throw new ArgumentNullException("dataBindSetter");
			}

			_dataBindSetter = dataBindSetter;
		}


		private readonly Action<IDataBind> _dataBindSetter;


		/// <summary>
		/// Привязка константы.
		/// </summary>
		/// <param name="value">Значение константы.</param>
		public void Constant(string value)
		{
			_dataBindSetter(new ConstantBind { Value = value });
		}

		/// <summary>
		/// Привязка параметра.
		/// </summary>
		/// <param name="name">Наименование параметра.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Parameter(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			_dataBindSetter(new ParameterBind { Parameter = name });
		}

		/// <summary>
		/// Привязка итога.
		/// </summary>
		/// <param name="name">Наименование итога.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Total(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
			{
				throw new ArgumentNullException("name");
			}

			_dataBindSetter(new TotalBind { Total = name });
		}

		/// <summary>
		/// Привязка свойства.
		/// </summary>
		/// <param name="dataSourceName">Наименование источника данных.</param>
		/// <param name="propertyPath">Путь к свойству источника данных.</param>
		/// <exception cref="ArgumentNullException"></exception>
		public void Property(string dataSourceName, string propertyPath)
		{
			if (string.IsNullOrWhiteSpace(dataSourceName))
			{
				throw new ArgumentNullException("dataSourceName");
			}

			if (string.IsNullOrWhiteSpace(propertyPath))
			{
				throw new ArgumentNullException("propertyPath");
			}

			_dataBindSetter(new PropertyBind { DataSource = dataSourceName, Property = propertyPath });
		}
	}
}