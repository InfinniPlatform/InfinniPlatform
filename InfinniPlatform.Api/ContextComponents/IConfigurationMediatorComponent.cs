using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.ContextComponents
{
	/// <summary>
	///   Контракт для связывания метаданных конфигурации и документов конфигурации в глобальном контексте
	/// </summary>
	public interface IConfigurationMediatorComponent
	{
		/// <summary>
		///  Объект конфигурации метаданных для скриптового доступа
		/// </summary>
		IConfigurationObject GetConfiguration(string configurationId);

		/// <summary>
		///  Конструктор объектов конфигураций для скриптового доступа
		/// </summary>
		IConfigurationObjectBuilder ConfigurationBuilder { get; }
	}
}
