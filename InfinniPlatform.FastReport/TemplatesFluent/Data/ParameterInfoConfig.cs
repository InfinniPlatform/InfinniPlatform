using System;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки информации о параметре отчета.
	/// </summary>
	public sealed class ParameterInfoConfig
	{
		internal ParameterInfoConfig(ParameterInfo parameterInfo)
		{
			if (parameterInfo == null)
			{
				throw new ArgumentNullException("parameterInfo");
			}

			_parameterInfo = parameterInfo;
		}


		private readonly ParameterInfo _parameterInfo;


		/// <summary>
		/// Отображаемый заголовок параметра.
		/// </summary>
		public ParameterInfoConfig Caption(string value)
		{
			_parameterInfo.Caption = value;

			return this;
		}

		/// <summary>
		/// Разрешить ввод пустого значения.
		/// </summary>
		public ParameterInfoConfig AllowNullValue()
		{
			_parameterInfo.AllowNullValue = true;

			return this;
		}

		/// <summary>
		/// Разрешить указание нескольких значений.
		/// </summary>
		public ParameterInfoConfig AllowMultiplyValues()
		{
			_parameterInfo.AllowMultiplyValues = true;

			return this;
		}

		/// <summary>
		/// Доступные значения для выбора.
		/// </summary>
		public ParameterInfoConfig AvailableValues(Action<ParameterValueProviderInfoConfig> action)
		{
			var configuration = new ParameterValueProviderInfoConfig();
			action(configuration);

			_parameterInfo.AvailableValues = configuration.ValueProviderInfo;

			return this;
		}

		/// <summary>
		/// Выбранные значения по умолчанию.
		/// </summary>
		public ParameterInfoConfig DefaultValues(Action<ParameterValueProviderInfoConfig> action)
		{
			var configuration = new ParameterValueProviderInfoConfig();
			action(configuration);

			_parameterInfo.DefaultValues = configuration.ValueProviderInfo;

			return this;
		}
	}
}