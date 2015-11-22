using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;

using Autofac;
using InfinniPlatform.Api.RestQuery;
using InfinniPlatform.ContextComponents;
using InfinniPlatform.Hosting;
using InfinniPlatform.Logging;
using InfinniPlatform.Sdk.ContextComponents;
using InfinniPlatform.WebApi.Controllers;
using InfinniPlatform.WebApi.Logging;

namespace InfinniPlatform.WebApi.WebApi
{
	/// <summary>
	/// Осуществляет хостинг приложения.
	/// </summary>
	public sealed class HostServer
	{
		public HostServer()
		{
		    _hostConfiguration = new HttpConfiguration();
		    _hostConfiguration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
			_hostConfiguration.Services.Replace(typeof(IHttpControllerSelector), new HttpControllerSelector(_hostConfiguration));

			_containerBuilder = new ContainerBuilder();
			_apiControllerFactory = new ApiControllerFactory(() => _container);
			_containerBuilder.RegisterInstance(ApiControllerFactory).AsSelf().AsImplementedInterfaces().SingleInstance();


			_containerBuilder.RegisterType<StandardApiController>().AsSelf();
		    _containerBuilder.RegisterType<UploadController>().AsSelf();
			_containerBuilder.RegisterType<UrlEncodedDataController>().AsSelf();


			RemoveXmlFormatter();
			SetupLogExceptionFilter();
		}

		private void RemoveXmlFormatter()
		{
			var appXmlType = _hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");

			if (appXmlType != null)
			{
				_hostConfiguration.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
			}

			_hostConfiguration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
		}

		private void SetupLogExceptionFilter()
		{
			_hostConfiguration.Filters.Add(new LogExceptionFilterAttribute(Logger.Log));
		}


		private readonly HttpConfiguration _hostConfiguration;

		/// <summary>
		/// Конфигурация хостинг-сервера.
		/// </summary>
		public HttpConfiguration HostConfiguration
		{
			get { return _hostConfiguration; }
		}


		private readonly IEnumerable<Assembly> _modules;

		/// <summary>
		/// Список модулей для хостинга.
		/// </summary>
		public IEnumerable<Assembly> Modules
		{
			get { return _modules; }
		}


		private readonly ContainerBuilder _containerBuilder;

		/// <summary>
		/// Фабрика для конфигурирования IoC-контейнера.
		/// </summary>
		public ContainerBuilder ContainerBuilder
		{
			get { return _containerBuilder; }
		}


		private readonly ApiControllerFactory _apiControllerFactory;

        /// <summary>
        /// Запустить хостинг-сервер.
        /// </summary>
        public void BuildServer()
        {
            BuildDependencies();

        }


		private Action _actionBefore;
		private Action _actionAfter;
		private Action<ContainerBuilder> _actionUpdate;
		private IContainer _container;

		/// <summary>
		/// Метод для получения IoC-контейнера.
		/// </summary>
		public Func<IContainer> Container
		{
			get { return () => _container; }
		}

	    public ApiControllerFactory ApiControllerFactory
	    {
	        get { return _apiControllerFactory; }
	    }

	    /// <summary>
		/// Инициализировать IoC-контейнер.
		/// </summary>
		private void BuildDependencies()
	    {
			if (_actionBefore != null)
			{
				_actionBefore.Invoke();
			}

			_container = ContainerBuilder.Build();

			if (_actionAfter != null)
			{
				_actionAfter.Invoke();
			}

			if (_actionUpdate != null)
			{
				var containerBuilderUpdate = new ContainerBuilder();
				_actionUpdate.Invoke(containerBuilderUpdate);
				containerBuilderUpdate.Update(_container);
			}
			
			//регистрируем сам контейнер зависимостей для возможности дальнейшего апдейта зависимостей пользователем
			var containerSelfBuilder = new ContainerBuilder();
			
			containerSelfBuilder.RegisterInstance<IDependencyContainerComponent>(new DependencyContainerComponent(
					regType => containerSelfBuilder.RegisterType(regType),
					instance => containerSelfBuilder.RegisterInstance(instance),
					type => _container.Resolve(type),
					() => containerSelfBuilder.Update(_container)
				));

			containerSelfBuilder.Update(_container);


			HostConfiguration.DependencyResolver = new AutofacWebApiDependencyResolver(_container);

		}

		/// <summary>
		/// Зарегистрировать действие, которое будет вызываться перед инициализацией IoC-контейнера. 
		/// </summary>
		/// <param name="actionBefore">action to invoke before initializing IoC container</param>
		public void BeforeCreateContainer(Action actionBefore)
		{
			_actionBefore = (Action)Delegate.Combine(_actionBefore, actionBefore);
		}

		/// <summary>
		/// Зарегистрироват действие, которое будет вызываться после инициализации IoC-контейнера.
		/// </summary>
		public void AfterCreateContainer(Action actionAfter)
		{
			_actionAfter = (Action)Delegate.Combine(_actionAfter, actionAfter);
		}

		/// <summary>
		/// Создать шаблон REST-сервиса.
		/// </summary>
		public IRestVerbsRegistrator CreateTemplate(string version, string metadataConfigurationId, string apiControllerName)
		{
			return ApiControllerFactory.CreateTemplate(version, metadataConfigurationId, apiControllerName);
		}

		/// <summary>
		///   Обновить регистрацию зависимостей в контейнере
		/// </summary>
		public void ActionUpdateContainer(Action<ContainerBuilder>  actionUpdate)
		{
			_actionUpdate = (Action<ContainerBuilder>) Delegate.Combine(_actionUpdate, actionUpdate);
		}

        /// <summary>
        ///   Регистрация роутинга конфигурации
        /// </summary>
        /// <param name="routeConfig">роутинг конфигурации</param>
	    public void RegisterRoutes(Action<HttpRouteCollection> routeConfig)
	    {
	        if (routeConfig != null)
	        {
	            routeConfig.Invoke(_hostConfiguration.Routes);
	        }
	    }

	    /// <summary>
	    ///   Удаление регистрации сервисов
	    /// </summary>
	    /// <param name="version">Версия конфигурации</param>
	    /// <param name="metadataConfigurationId">Идентификатор конфигурации, для которой выполянем удаление регистраций</param>
	    public void RemoveTemplates(string version, string metadataConfigurationId)
		{
			ApiControllerFactory.RemoveTemplates(version, metadataConfigurationId);
		}
	}
}