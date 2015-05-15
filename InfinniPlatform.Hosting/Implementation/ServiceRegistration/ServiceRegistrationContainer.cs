using System;
using System.Collections.Generic;
using System.Linq;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Hosting.Implementation.ExtensionPointHandling;

namespace InfinniPlatform.Hosting.Implementation.ServiceRegistration
{

	/// <summary>
	///  Контейнер зарегистрированных шаблонов конфигурации
	/// </summary>
	public sealed class ServiceRegistrationContainer : IServiceRegistrationContainer
	{
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;
		private readonly string _metadataConfigurationId;
		private readonly List<IServiceRegistration> _serviceRegistrations = new List<IServiceRegistration>(); 

		public ServiceRegistrationContainer(IServiceTemplateConfiguration serviceTemplateConfiguration, string metadataConfigurationId)
		{
			_serviceTemplateConfiguration = serviceTemplateConfiguration;
			_metadataConfigurationId = metadataConfigurationId;
		}

		/// <summary>
		///  Добавить регистрацию сервиса
		/// </summary>
		/// <param name="metadata">Идентификатор метаданных</param>
		/// <param name="serviceName">Наименование сервиса</param>
		/// <param name="actionHandler">Конструктор обработчика</param>
		/// <param name="registrationAction">Инициализация регистрации</param>
		/// <returns></returns>
		public IServiceRegistrationContainer AddRegistration(string metadata, string serviceName, Action<IServiceRegistration> registrationAction = null, Func<string, IExtensionPointHandler> actionHandler = null)
		{
			//необходимо добавить проверку уникальности сервисов

			var registration = new ServiceRegistration( metadata, serviceName, _serviceTemplateConfiguration.GetServiceHandler(serviceName));

			registration.RegisterService(actionHandler);

			_serviceRegistrations.Add(registration);

			registration.RegisterHandlerInstance(serviceName);

			if (registrationAction != null)
			{
				registrationAction.Invoke(registration);
			}
			return this;
		}

		/// <summary>
		///   Список зарегистрированных сервисов
		/// </summary>
		public IEnumerable<IServiceRegistration> Registrations
		{
			get { return _serviceRegistrations; }
		}

		/// <summary>
		///   Идентификатор конфигурации, которой принадлежит контейнер
		/// </summary>
		public string MetadataConfigurationId
		{
			get { return _metadataConfigurationId; }
		}

		/// <summary>
		///   Получить зарегистрированный сервис
		/// </summary>
		/// <param name="metadata">идентификатор метаданных</param>
		/// <param name="service">наименование обработчика</param>
		/// <returns></returns>
		public IServiceRegistration GetRegistration(string metadata, string service)
		{
			return _serviceRegistrations.FirstOrDefault(sr => sr.MetadataName.ToLowerInvariant() == metadata.ToLowerInvariant() && sr.ServiceName.ToLowerInvariant() == service.ToLowerInvariant());
		}

		/// <summary>
		///  Получить первый экземпляр обработчика зарегистрированный под указанным именем сервиса
		/// </summary>
		/// <param name="metadata"></param>
		/// <param name="serviceInstanceName"></param>
		/// <returns></returns>
		public IExtensionPointHandlerInstance GetRegistrationByInstanceName(string metadata, string serviceInstanceName)
		{
			var registration = _serviceRegistrations.Where(sr => sr.MetadataName.ToLowerInvariant() == metadata.ToLowerInvariant()).ToList();
			if (registration.Count > 0)
			{
				foreach (var serviceRegistration in registration)
				{
					var handler = serviceRegistration.ExtensionPointHandler.GetHandlerInstance(serviceInstanceName);	
					if (handler != null)
					{
						return handler;
					}
				}
				
			}
			return null;
		}
	}
}