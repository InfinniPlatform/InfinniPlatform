﻿using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Factories;

namespace InfinniPlatform.Hosting.Implementation.ServiceRegistration
{
	public sealed class ServiceRegistrationContainerFactory : IServiceRegistrationContainerFactory
	{
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

		public ServiceRegistrationContainerFactory(IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			_serviceTemplateConfiguration = serviceTemplateConfiguration;
		}

		public IServiceTemplateConfiguration ServiceTemplateConfiguration
		{
			get { return _serviceTemplateConfiguration; }
		}

		public IServiceRegistrationContainer BuildServiceRegistrationContainer(string metadataConfigurationId)
		{
			return new ServiceRegistrationContainer(ServiceTemplateConfiguration,metadataConfigurationId);
		}
	}
}
