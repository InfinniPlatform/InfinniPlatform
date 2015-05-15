using InfinniPlatform.Hosting.WebApi.Implementation.ServiceTemplates;

namespace InfinniPlatform.Hosting.WebApi.Implementation.ServiceRegistration
{
	public sealed class ServiceRegistrationContainerProvider : IServiceRegistrationContainerProvider
	{
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

		public ServiceRegistrationContainerProvider(IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			_serviceTemplateConfiguration = serviceTemplateConfiguration;
		}

		public IServiceRegistrationContainer CreateServiceRegistrationContainer()
		{
			return new ServiceRegistrationContainer(_serviceTemplateConfiguration);
		}
	}
}
