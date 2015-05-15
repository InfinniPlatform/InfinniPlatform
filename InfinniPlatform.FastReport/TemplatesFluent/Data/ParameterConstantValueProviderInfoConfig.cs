using System;
using System.Collections.Generic;

using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки информации о поставщике значений параметров отчета в виде предопределенного списка.
	/// </summary>
	public sealed class ParameterConstantValueProviderInfoConfig
	{
		internal ParameterConstantValueProviderInfoConfig(ParameterConstantValueProviderInfo valueProviderInfo)
		{
			if (valueProviderInfo == null)
			{
				throw new ArgumentNullException("valueProviderInfo");
			}

			_valueProviderInfo = valueProviderInfo;
		}


		private readonly ParameterConstantValueProviderInfo _valueProviderInfo;


		/// <summary>
		/// Добавить элемент в список значений параметра.
		/// </summary>
		public ParameterConstantValueProviderInfoConfig AddValue(string label, object constant)
		{
			if (label == null)
			{
				throw new ArgumentNullException("label");
			}

			if (_valueProviderInfo.Items == null)
			{
				_valueProviderInfo.Items = new Dictionary<string, IDataBind>();
			}

			_valueProviderInfo.Items[label] = new ConstantBind { Value = constant };

			return this;
		}
	}
}