using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata.Implementation.HostServerConfiguration;
using InfinniPlatform.Modules;

namespace InfinniPlatform.Metadata.Implementation.Modules
{
	/// <summary>
	///   Регистратор стандартных шаблонов обработчиков запроса
	/// </summary>
	public sealed class StandardTemplatesInstaller : ITemplateInstaller
	{
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

		public StandardTemplatesInstaller(IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			_serviceTemplateConfiguration = serviceTemplateConfiguration;
		}

		/// <summary>
		///   Зарегистрировать шаблоны в конфигурации
		/// </summary>
		public void RegisterTemplates()
		{
			_serviceTemplateConfiguration.CreateDefaultServiceConfiguration();
		}
	}
}
