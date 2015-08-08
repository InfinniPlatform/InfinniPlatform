using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using Autofac;
using Autofac.Configuration;
using InfinniPlatform.Api.Hosting;
using InfinniPlatform.Api.Metadata;
using InfinniPlatform.Factories;
using InfinniPlatform.Hosting;
using InfinniPlatform.Metadata;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.Sdk.Environment;
using InfinniPlatform.Sdk.Environment.Hosting;
using InfinniPlatform.Sdk.Environment.Metadata;
using InfinniPlatform.Sdk.Environment.Scripts;
using InfinniPlatform.WebApi.WebApi;

namespace InfinniPlatform.WebApi.Factories
{

	/// <summary>
	///   Сервер хостинга конфигураций
	/// </summary>
	public class InfinniPlatformHostServer
	{
		private readonly HostServer _hostServer;
		private readonly IServiceRegistrationContainerFactory _serviceRegistrationContainerFactory;
		private readonly ModuleComposer _moduleComposer;
		private readonly IServiceTemplateConfiguration _serviceTemplateConfiguration;

		private InfinniPlatformHostServer(HostServer hostServer, IServiceRegistrationContainerFactory serviceRegistrationContainerFactory, IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			_hostServer = hostServer;
			_serviceRegistrationContainerFactory = serviceRegistrationContainerFactory;

			_serviceTemplateConfiguration = serviceTemplateConfiguration;
			_moduleComposer = new ModuleComposer(_hostServer.Modules, hostServer.ContainerBuilder, hostServer.Container);

		}


		public static InfinniPlatformHostServer Instance { get; private set; }

		/// <summary>
		///  В пределах одного домена возможен запуск только одного сервера приложения.
		///  Цель: обеспечить возможность доступа к свойствам и методам серверы платформы
		/// без дополнительных затрат на обеспечение доступа к нему в прикладных скриптах
		/// </summary>
		/// <param name="hostServer">Сервер хостинга</param>
		/// <param name="containerFactory">Фабрика регистрации сервисов</param>
		/// <param name="serviceTemplateConfiguration">Конфигурация шаблонов сервисов</param>
		/// <returns>Сконфигурированный сервер</returns>
		public static InfinniPlatformHostServer CreateInstance(HostServer hostServer, IServiceRegistrationContainerFactory containerFactory, IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			Instance = new InfinniPlatformHostServer(hostServer, containerFactory, serviceTemplateConfiguration);
			return Instance;
		}

		/// <summary>
		///  Список модулей, загружаемых при инициализации сервера конфигурации
		///  Список модулей используется в процедурах регистрации зависимостей,
		///  регистрации индексирующих сервисов, регистрации сервисов терминологии
		/// </summary>
		public IEnumerable<Assembly> Modules
		{
			get { return _hostServer.Modules; }
		}

		public HttpConfiguration HttpConfiguration
		{
			get { return _hostServer.HostConfiguration; }
		}


		/// <summary>
		///   Конфигурация шаблонов сервисов для данного сервера
		/// </summary>
		public IServiceTemplateConfiguration ServiceTemplateConfiguration
		{
			get { return _serviceTemplateConfiguration; }
		}

		/// <summary>
		///   Конфигурация роутинга по умолчанию. 
		///   Сервер может быть сконфигурирован с использованием роутинга по умолчанию, 
		///   с помощью вызова метода InstallDefaultRoutes, либо 
		///   с помощью вызова метода InstallRoutes с указанием другой конфигурации роутинга 
		/// </summary>
		private static readonly Action<HttpRouteCollection> RouteConfig = routes =>
		{
            routes.MapHttpRoute("DefaultApi", "api/{controller}/{action}");
            routes.MapHttpRoute("Default", "api/{controller}/{id}", new { id = RouteParameter.Optional });
            routes.MapHttpRoute("DefaultWithAction", "api/{controller}/{action}/{id}", new { id = RouteParameter.Optional });
            routes.MapHttpRoute("RestControllerConfiguration", "{controller}/{service}", new
			{
				service = RouteParameter.Optional,
				id = RouteParameter.Optional
			});

            routes.MapHttpRoute("RestControllerDefault", "{configuration}/{controller}/{metadata}/{service}/{id}", new { id = RouteParameter.Optional });

		};

		/// <summary>
		///   Ручная установка сервисов WebApi
		///   По умолчанию, установка сервисов осуществляется автоматически при поиске реализации
		///   IModuleInstaller и вызове его методов регистрации сервисов, однако, возможна
		///   ручная регистрация методов установки сервисов, позволяющая регистрировать дополнительные сервисы,
		///   не связанные с каким-либо установщиком модуля. При этом следует иметь в виду, что регистрация
		///   для одного и того же сервера нескольких типов сервисов с одним и тем же списком параметров 
		///   приведет к невозможности разрешения запускаемого метода.
		/// </summary>		
		public InfinniPlatformHostServer InstallServices(string version, IServiceRegistrationContainer serviceRegistrationContainer)
		{
            foreach (var serviceType in serviceRegistrationContainer.Registrations)
			{
				_hostServer.CreateTemplate(version, serviceRegistrationContainer.MetadataConfigurationId, serviceType.MetadataName).AddVerb(serviceType.QueryHandler);
			}
			return this;
		}

        public InfinniPlatformHostServer RegisterVersion(string metadataConfigurationId, string version)
        {
            _hostServer.ApiControllerFactory.RegisterVersion(metadataConfigurationId, version);
            return this;
        }

        public InfinniPlatformHostServer UnregisterVersion(string metadataConfigurationId, string version)
        {
            _hostServer.ApiControllerFactory.UnregisterVersion(metadataConfigurationId, version);
            return this;
        }

		/// <summary>
		///   Удаление установленных сервисов (обычно используется для переустановки модуля в режиме runtime)
		/// </summary>
		public void UninstallServices(string version, string metadataConfigurationId)
		{
            _hostServer.RemoveTemplates(version, metadataConfigurationId);
		}


		/// <summary>
		///   Установка роутинга по умолчанию, примеяемого для всех конфигураций, хостящихся на данном сервере
		/// </summary>		
		private InfinniPlatformHostServer InstallDefaultRoutes()
		{
			_hostServer.BeforeCreateContainer(() =>
				_hostServer.RegisterRoutes(RouteConfig));
			return this;
		}

		/// <summary>
		///   Регистрация конфигураций метаданных.
		///   Осуществляет поиск установщиков модулей на сервере и затем регистрирует метаданные для каждого из них 
		/// </summary>
		/// <returns></returns>
		private InfinniPlatformHostServer InstallModules()
		{
			_hostServer.AfterCreateContainer(() =>
			{
				_moduleComposer.RegisterTemplates();
				_hostServer.ActionUpdateContainer(updateContainer =>
				{
					foreach (var registeredType in ServiceTemplateConfiguration.GetRegisteredTypes())
					{
						updateContainer.RegisterType(registeredType).AsSelf().InstancePerDependency();
					}
				});


				var modules = _moduleComposer.RegisterModules();
				foreach (var module in modules)
				{
					InstallServices(module.Version, module.ServiceRegistrationContainer);
				}
			});

			return this;
		}

		/// <summary>
		///   Сконструировать сервер хостинга конфигурации исходя из настроек по умолчанию
		/// </summary>
		/// <param name="assemblies">Список модулей, в которых будет осуществляться поиск установщиков модулей конфигураций</param>
		/// <param name="containerFactory">Фабрика контейнеров регистрации сервисов</param>
		/// <param name="serviceTemplateConfiguration">Конфигурация сервисов по умолчанию</param>
		/// <returns>Сконфигурированный настройками по умолчанию сервер</returns>
		public static InfinniPlatformHostServer ConstructHostServer(IEnumerable<Assembly> assemblies, IServiceRegistrationContainerFactory containerFactory,
			IServiceTemplateConfiguration serviceTemplateConfiguration)
		{
			assemblies = assemblies.ToList();
			var server = new HostServer(assemblies);

			var hostServer = CreateInstance(server, containerFactory, serviceTemplateConfiguration)
				.InstallDefaultRoutes()
				.InstallDefaultConfigurationDependencies()
				.InstallModules();
			return hostServer;
		}

		/// <summary>
		///   Установить зависимости по умолчанию для сервера конфигураций
		/// </summary>
		/// <returns></returns>
		private InfinniPlatformHostServer InstallDefaultConfigurationDependencies()
		{
			_hostServer.BeforeCreateContainer(() =>
			{
				var containerBuilder = _hostServer.ContainerBuilder;
				containerBuilder.RegisterInstance(ServiceTemplateConfiguration).AsSelf().AsImplementedInterfaces().SingleInstance();
				containerBuilder.RegisterInstance(_serviceRegistrationContainerFactory).AsImplementedInterfaces().AsSelf().SingleInstance();
				containerBuilder.RegisterModule(new XmlFileReader("Autofac.xml"));
			});
			return this;
		}


		public void Build()
		{
			_hostServer.BuildServer();
		}

	    /// <summary>
	    ///   Создать экземпляр пустой (неинициализированной) конфигурации на сервере
	    /// </summary>
	    /// <param name="version">Версия приложения</param>
	    /// <param name="configurationId">Идентификатор конфигурации</param>
	    /// <param name="isEmbeddedConfiguration">Признак встроенной конфигурации C#</param>
	    /// <returns>Конфигурация метаданных</returns>
	    public IMetadataConfiguration CreateConfiguration(string version, string configurationId, bool isEmbeddedConfiguration)
		{
			var metadataConfigurationProvider = _hostServer.Container().Resolve<IMetadataConfigurationProvider>();
			var actionConfig = _hostServer.Container().Resolve<IScriptConfiguration>();

            Instance.RegisterVersion(configurationId, version);

			return metadataConfigurationProvider.AddConfiguration(version, configurationId, actionConfig, isEmbeddedConfiguration);
		}

	    /// <summary>
	    ///   Удалить конфигурацию из списка сервера
	    /// </summary>
	    /// <param name="version">Версия приложения</param>
	    /// <param name="configurationId">Идентификатор конфигурации</param>
	    public void RemoveConfiguration(string version, string configurationId)
		{
			var metadataConfigurationProvider = _hostServer.Container().Resolve<IMetadataConfigurationProvider>();
			metadataConfigurationProvider.RemoveConfiguration(version, configurationId);
		}

		private readonly IList<Type> _startupInitializers = new List<Type>();

		/// <summary>
		///   Зарегистрировать тип инициализатора старта сервера
		/// </summary>
		public void RegisterServerInitializer<T>() where T : IStartupInitializer
		{
			_startupInitializers.Add(typeof(T));
			_hostServer.ContainerBuilder.RegisterType<T>().AsSelf().AsImplementedInterfaces();
		}

		/// <summary>
		///   При старте хостинга 
		/// </summary>
		public void OnStartHost(HostingContextBuilder contextBuilder, IHostingContext context)
		{

			foreach (var startupInitializer in _startupInitializers)
			{
				((IStartupInitializer)_hostServer.Container().Resolve(startupInitializer)).OnStart(contextBuilder);
			}
		}

		public ApiControllerFactory GetApiControllerFactory()
		{
			return _hostServer.ApiControllerFactory;
		}
	}


}
