using System;
using System.Collections.Generic;
using InfinniPlatform.Api.Schema;
using InfinniPlatform.FastReport.Templates.Data;

namespace InfinniPlatform.FastReport.TemplatesFluent.Data
{
	/// <summary>
	/// Интерфейс для настройки параметров отчета.
	/// </summary>
	public sealed class ParametersConfig
	{
		internal ParametersConfig(ICollection<ParameterInfo> parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}

			_parameters = parameters;
		}


		private readonly ICollection<ParameterInfo> _parameters;


		/// <summary>
		/// Зарегистрировать параметр.
		/// </summary>
		public ParametersConfig Register(string name, SchemaDataType type, Action<ParameterInfoConfig> options = null)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentNullException("type");
			}

			var parameterInfo = new ParameterInfo { Type = type, Name = name };

			if (options != null)
			{
				var configuration = new ParameterInfoConfig(parameterInfo);
				options(configuration);
			}

			_parameters.Add(parameterInfo);

			return this;
		}
	}
}