using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting;

namespace InfinniPlatform.Factories
{
	public interface IServiceRegistrationContainerFactory
	{
		IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId);
		IServiceTemplateConfiguration ServiceTemplateConfiguration { get; }
	}
}
