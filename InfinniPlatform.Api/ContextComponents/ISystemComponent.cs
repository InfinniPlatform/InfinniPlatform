using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Metadata;

namespace InfinniPlatform.Api.ContextComponents
{
	public interface ISystemComponent
	{
		/// <summary>
		///   Менеджер идентификаторов конфигураций
		/// </summary>
		IManagerIdentifiers ManagerIdentifiers { get; set; }

		/// <summary>
		///   Контракт для чтения метаданных конфигурации
		/// </summary>
		IJsonConfigReader ConfigurationReader { get; set; }
	}
}
